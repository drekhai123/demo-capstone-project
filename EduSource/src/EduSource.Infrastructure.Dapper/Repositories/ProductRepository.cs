using Dapper;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Services.Products;
using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Text;

namespace EduSource.Infrastructure.Dapper.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;
    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product>? GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> GetDetailsAsync(Guid productId)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            // Query products and their images
            var productData = new Dictionary<Guid, Product>();

            await connection.QueryAsync<Product, ImageOfProduct, Book, Product>(
                @"SELECT p.Id, p.Name, p.Price, p.Category, p.Description, p.ContentType, p.Unit, p.UploadType, p.TotalPage, p.Size, p.ImageUrl, p.FileUrl, p.FileDemoUrl, p.Rating, p.IsPublic, p.IsApproved, p.CreatedDate, p.ModifiedDate AS ProductModifiedDate,  
                iop.ImageUrl, iop.ImageId, iop.CreatedDate AS ImageCreatedDate,
                b.Id, b.Name, b.ImageUrl, b.GradeLevel, b.Category
                FROM Products p
                LEFT JOIN ImageOfProducts iop ON p.Id = iop.ProductId
                JOIN Books b ON b.Id = p.BookId
                WHERE p.Id = @Id",
                (product, productImage, book) =>
                {
                    if (!productData.TryGetValue(product.Id, out var existingProduct))
                    {
                        // If this product is not yet added, create it
                        existingProduct = product;
                        existingProduct.UpdateImageOfProducts(new List<ImageOfProduct>());
                        productData.Add(existingProduct.Id, existingProduct);
                    }

                    // Add images to the product object
                    if (productImage != null && !existingProduct.ImageOfProducts.Any(img => img.ImageId == productImage.ImageId))
                    {
                        existingProduct.ImageOfProducts.Add(productImage);
                    }
                    existingProduct.UpdateBook(book);
                    return existingProduct;
                },
                new { Id = productId },
                splitOn: "ProductModifiedDate, ImageCreatedDate");

            return productData.Values.ToList()[0];
        }
    }

    public async Task<Product> GetDetailsByUserAsync(Guid productId, Guid accountId)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            // Query products and their images
            var productData = new Dictionary<Guid, Product>();

            await connection.QueryAsync<Product, ImageOfProduct, OrderDetails, Order, Book, Product>(
                @"SELECT p.Id, p.Name, p.Price, p.Category, p.Description, p.ContentType, p.Unit, p.UploadType, p.TotalPage, p.Size, p.ImageUrl, p.FileUrl, p.FileDemoUrl, p.Rating, p.IsPublic, p.IsApproved, p.CreatedDate, p.ModifiedDate AS ProductModifiedDate,  
                iop.ImageUrl, iop.ImageId, iop.CreatedDate AS ImageCreatedDate,
                od.Id, od.CreatedDate AS OrderDetailsCreatedDate,
                o.Id, o.AccountId, o.CreatedDate AS OrderCreatedDate,
                b.Id, b.Name, b.ImageUrl, b.GradeLevel, b.Category
                FROM Products p
                LEFT JOIN ImageOfProducts iop ON p.Id = iop.ProductId
                LEFT JOIN OrderDetails od ON p.Id = od.ProductId
                JOIN Orders o ON o.Id = od.OrderId
                JOIN Books b ON b.Id = p.BookId
                WHERE p.Id = @Id",
                (product, productImage, orderDetails, order, book) =>
                {
                    if (!productData.TryGetValue(product.Id, out var existingProduct))
                    {
                        // If this product is not yet added, create it
                        existingProduct = product;
                        existingProduct.UpdateImageOfProducts(new List<ImageOfProduct>());
                        existingProduct.UpdateOrderDetails(new List<OrderDetails>());
                        productData.Add(existingProduct.Id, existingProduct);
                    }

                    // Add images to the product object
                    if (productImage != null && !existingProduct.ImageOfProducts.Any(img => img.ImageId == productImage.ImageId))
                    {
                        existingProduct.ImageOfProducts.Add(productImage);
                    }
                    // Add orderDetails to the product object
                    if (orderDetails != null && !existingProduct.OrderDetails.Any(od => od.Id == orderDetails.Id))
                    {
                        if (order.AccountId == accountId)
                        {
                            orderDetails.UpdateOrder(order);
                            existingProduct.OrderDetails.Add(orderDetails);
                        }

                    }
                    existingProduct.UpdateBook(book);
                    return existingProduct;
                },
                new { Id = productId },
                splitOn: "ProductModifiedDate, ImageCreatedDate, OrderDetailsCreatedDate, OrderCreatedDate");

            return productData.Values.ToList()[0];
        }
    }

    public async Task<PagedResult<Product>> GetPagedAsync(int pageIndex, int pageSize, Filter.ProductFilter filterParams, string[] selectedColumns)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            // Valid columns for selecting
            var validColumns = new HashSet<string> { "Id", "Name", "Price", "Category", "Unit", "ContentType", "UploadType", "TotalPage", "Size", "ImageUrl", "FileUrl", "FileDemoUrl", "Rating", "IsPublic", "IsApproved" };
            var columns = selectedColumns?.Where(c => validColumns.Contains(c)).ToArray();

            // If no selected columns, select all
            var selectedColumnsString = columns?.Length > 0 ? string.Join(", ", columns) : string.Join(", ", validColumns); ;

            // Start building the query
            var queryBuilder = new StringBuilder($"SELECT {selectedColumnsString} FROM Products WHERE 1=1 AND IsDeleted = 0");

            var parameters = new DynamicParameters();

            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize > 100 ? 100 : pageSize;

            var totalCountQuery = new StringBuilder($@"
        SELECT COUNT(1) 
        FROM Products p
        WHERE 1=1 AND IsDeleted = 0");

            // Filter by Name
            if (!string.IsNullOrEmpty(filterParams?.Name))
            {
                queryBuilder.Append(" AND Name LIKE @Name");
                totalCountQuery.Append(" AND Name LIKE @Name");
                parameters.Add("Name", $"%{filterParams.Name}%");
            }

            //Filter by Price
            if (filterParams?.Price.HasValue == true)
            {
                queryBuilder.Append(" AND Price = @Price");
                totalCountQuery.Append(" AND Price = @Price");
                parameters.Add("Price", $"{filterParams.Price}");
            }

            //Filter by Category
            if (filterParams?.Category.HasValue == true)
            {
                queryBuilder.Append(" AND Category = @Category");
                totalCountQuery.Append(" AND Category = @Category");
                parameters.Add("Category", $"{(int)filterParams.Category}");
            }

            //Filter by Description
            if (!string.IsNullOrEmpty(filterParams?.Description))
            {
                queryBuilder.Append(" AND Description LIKE @Description");
                totalCountQuery.Append(" AND Description LIKE @Description");
                parameters.Add("Description", $"%{filterParams.Description}%");
            }

            //Filter by ContentType
            if (filterParams?.ContentType.HasValue == true)
            {
                queryBuilder.Append(" AND ContentType = @ContentType");
                totalCountQuery.Append(" AND ContentType = @ContentType");
                parameters.Add("ContentType", $"{(int)filterParams.ContentType}");
            }

            //Filter by Unit
            if (filterParams?.Unit.HasValue == true)
            {
                queryBuilder.Append(" AND Unit = @Unit");
                totalCountQuery.Append(" AND Unit = @Unit");
                parameters.Add("Unit", $"{filterParams.Unit}");
            }

            //Filter by UploadType
            if (filterParams?.UploadType.HasValue == true)
            {
                queryBuilder.Append(" AND UploadType = @UploadType");
                totalCountQuery.Append(" AND UploadType = @UploadType");
                parameters.Add("UploadType", $"{(int)filterParams.UploadType}");
            }

            //Filter by TotalPage
            if (filterParams?.TotalPage.HasValue == true)
            {
                queryBuilder.Append(" AND TotalPage = @TotalPage");
                totalCountQuery.Append(" AND TotalPage = @TotalPage");
                parameters.Add("TotalPage", $"{filterParams.TotalPage}");
            }

            //Filter by Size
            if (filterParams?.Size.HasValue == true)
            {
                queryBuilder.Append(" AND Size = @Size");
                totalCountQuery.Append(" AND Size = @Size");
                parameters.Add("Size", $"{filterParams.Size}");
            }

            //Filter by Rating
            if (filterParams?.Rating.HasValue == true)
            {
                queryBuilder.Append(" AND Rating = @Rating");
                totalCountQuery.Append(" AND Rating = @Rating");
                parameters.Add("Rating", $"{filterParams.Rating}");
            }

            //Filter by IsPublic
            if (filterParams?.IsPublic.HasValue == true)
            {
                queryBuilder.Append(" AND IsPublic = @IsPublic");
                totalCountQuery.Append(" AND IsPublic = @IsPublic");
                parameters.Add("IsPublic", $"{filterParams.IsPublic}");
            }

            //Filter by IsApproved
            if (filterParams?.IsApproved.HasValue == true)
            {
                queryBuilder.Append(" AND IsApproved = @IsApproved");
                totalCountQuery.Append(" AND IsApproved = @IsApproved");
                parameters.Add("IsApproved", $"{filterParams.IsApproved}");
            }

            //Filter by BookId
            if (filterParams?.BookId.HasValue == true)
            {
                queryBuilder.Append(" AND BookId = @BookId");
                totalCountQuery.Append(" AND BookId = @BookId");
                parameters.Add("BookId", $"{filterParams.BookId}");
            }

            //Filter by StaffId
            if (filterParams?.StaffId.HasValue == true)
            {
                queryBuilder.Append(" AND AccountId = @StaffId");
                totalCountQuery.Append(" AND AccountId = @StaffId");
                parameters.Add("StaffId", $"{filterParams.StaffId}");
            }

            //Count TotalCount
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountQuery.ToString(), parameters);

            //Count TotalPages
            var totalPages = Math.Ceiling((totalCount / (double)pageSize));

            var offset = (pageIndex - 1) * pageSize;
            var paginatedQuery = $"{queryBuilder} ORDER BY Id OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            var items = (await connection.QueryAsync<Product>(paginatedQuery, parameters)).ToList();

            return new PagedResult<Product>(items, pageIndex, pageSize, totalCount, totalPages);

        }
    }

    public async Task<PagedResult<Product>> GetPagedByUserAsync(int pageIndex, int pageSize, Filter.ProductFilter filterParams, string[] selectedColumns)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            // Valid columns for selecting
            var validColumns = new HashSet<string> { "p.Id", "p.Name", "Price", "p.Category", "p.Unit", "p.ContentType", "p.UploadType", "p.TotalPage", "p.Size", "p.ImageUrl", "p.FileUrl", "p.FileDemoUrl", "p.Rating", "p.IsPublic", "p.IsApproved", "p.CreatedDate", "p.ModifiedDate AS ProductModifiedDate", "od.Id", "od.ProductId", "od.OrderId" };
            var columns = selectedColumns?.Where(c => validColumns.Contains(c)).ToArray();

            // If no selected columns, select all
            var selectedColumnsString = columns?.Length > 0 ? string.Join(", ", columns) : string.Join(", ", validColumns);

            // Start building the query
            var queryBuilder = new StringBuilder(
                $@"SELECT {selectedColumnsString} FROM Products p 
                LEFT JOIN OrderDetails od ON p.Id = od.ProductId
                WHERE 1=1 AND p.IsDeleted = 0");

            var parameters = new DynamicParameters();

            // Calculate pageIndex and pageSize 
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize > 100 ? 100 : pageSize;


            // Filter by Name
            if (!string.IsNullOrEmpty(filterParams?.Name))
            {
                queryBuilder.Append(" AND Name LIKE @Name");
                parameters.Add("Name", $"%{filterParams.Name}%");
            }

            //Filter by Price
            if (filterParams?.Price.HasValue == true)
            {
                queryBuilder.Append(" AND Price = @Price");
                parameters.Add("Price", $"{filterParams.Price}");
            }

            //Filter by Category
            if (filterParams?.Category.HasValue == true)
            {
                queryBuilder.Append(" AND Category = @Category");
                parameters.Add("Category", $"{(int)filterParams.Category}");
            }

            //Filter by Description
            if (!string.IsNullOrEmpty(filterParams?.Description))
            {
                queryBuilder.Append(" AND Description LIKE @Description");
                parameters.Add("Description", $"%{filterParams.Description}%");
            }

            //Filter by ContentType
            if (filterParams?.ContentType.HasValue == true)
            {
                queryBuilder.Append(" AND ContentType = @ContentType");
                parameters.Add("ContentType", $"{(int)filterParams.ContentType}");
            }

            //Filter by Unit
            if (filterParams?.Unit.HasValue == true)
            {
                queryBuilder.Append(" AND Unit = @Unit");
                parameters.Add("Unit", $"{filterParams.Unit}");
            }

            //Filter by UploadType
            if (filterParams?.UploadType.HasValue == true)
            {
                queryBuilder.Append(" AND UploadType = @UploadType");
                parameters.Add("UploadType", $"{(int)filterParams.UploadType}");
            }

            //Filter by TotalPage
            if (filterParams?.TotalPage.HasValue == true)
            {
                queryBuilder.Append(" AND TotalPage = @TotalPage");
                parameters.Add("TotalPage", $"{filterParams.TotalPage}");
            }

            //Filter by Size
            if (filterParams?.Size.HasValue == true)
            {
                queryBuilder.Append(" AND Size = @Size");
                parameters.Add("Size", $"{filterParams.Size}");
            }

            //Filter by Rating
            if (filterParams?.Rating.HasValue == true)
            {
                queryBuilder.Append(" AND Rating = @Rating");
                parameters.Add("Rating", $"{filterParams.Rating}");
            }

            //Filter by IsPublic
            if (filterParams?.IsPublic.HasValue == true)
            {
                queryBuilder.Append(" AND IsPublic = @IsPublic");
                parameters.Add("IsPublic", $"{filterParams.IsPublic}");
            }

            //Filter by IsApproved
            if (filterParams?.IsApproved.HasValue == true)
            {
                queryBuilder.Append(" AND IsApproved = @IsApproved");
                parameters.Add("IsApproved", $"{filterParams.IsApproved}");
            }

            //Filter by BookId
            if (filterParams?.BookId.HasValue == true)
            {
                queryBuilder.Append(" AND BookId = @BookId");
                parameters.Add("BookId", $"{filterParams.BookId}");
            }

            //Filter by StaffId
            if (filterParams?.StaffId.HasValue == true)
            {
                queryBuilder.Append(" AND AccountId = @StaffId");
                parameters.Add("StaffId", $"{filterParams.StaffId}");
            }

            // Query products and their orders
            var productData = new Dictionary<Guid, Product>();

            await connection.QueryAsync<Product, OrderDetails, Product>(
                queryBuilder.ToString(),
                (product, orderDetails) =>
                {
                    if (!productData.TryGetValue(product.Id, out var existingProduct))
                    {
                        // If this product is not yet added, create it
                        existingProduct = product;
                        existingProduct.UpdateOrderDetails(new List<OrderDetails>());
                        productData.Add(existingProduct.Id, existingProduct);
                    }
                    // Add orderDetails to the product object
                    if (orderDetails != null && !existingProduct.OrderDetails.Any(od => od.Id == orderDetails.Id))
                    {
                        if (product.Id == orderDetails.ProductId)
                        {
                            existingProduct.OrderDetails.Add(orderDetails);
                        }

                    }
                    return existingProduct;
                },
                parameters,
                splitOn: "ProductModifiedDate");

            //Result
            var result = productData.Values.ToList();
            // Count TotalCount, TotalPages and calculate offset
            int totalCount = result.Count;
            var totalPages = Math.Ceiling((totalCount / (double)pageSize));
            var offset = (pageIndex - 1) * pageSize;

            // Apply sorting

            // Apply pagination
            result = result.Skip(offset).Take(pageSize).ToList();
            return new PagedResult<Product>(result, pageIndex, pageSize, totalCount, totalPages);
        }
    }

    public async Task<PagedResult<Product>> GetProductsInCartAsync(int pageIndex, int pageSize, Filter.ProductFilter filterParams, string[] selectedColumns)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            // Valid columns for selecting
            var validColumns = new HashSet<string> { "p.Id", "p.Name", "Price", "p.Category", "p.Unit", "p.ContentType", "p.UploadType", "p.TotalPage", "p.Size", "p.ImageUrl", "p.FileUrl", "p.FileDemoUrl", "p.Rating", "p.IsPublic", "p.IsApproved" };
            var columns = selectedColumns?.Where(c => validColumns.Contains(c)).ToArray();

            // If no selected columns, select all
            var selectedColumnsString = columns?.Length > 0 ? string.Join(", ", columns) : string.Join(", ", validColumns); ;

            // Start building the query
            var queryBuilder = new StringBuilder(
                $@"SELECT {selectedColumnsString} FROM Products p 
                JOIN Carts c ON p.Id = c.ProductId
                WHERE 1=1 AND p.IsDeleted = 0 AND c.AccountId = @AccountId");

            var parameters = new DynamicParameters();

            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize > 100 ? 100 : pageSize;

            var totalCountQuery = new StringBuilder($@"
        SELECT COUNT(1) 
        FROM Products p
        JOIN Carts c ON p.Id = c.ProductId
        WHERE 1=1 AND p.IsDeleted = 0 AND c.AccountId = @AccountId");

            parameters.Add("AccountId", $"{filterParams.UserId}");

            // Filter by Name
            if (!string.IsNullOrEmpty(filterParams?.Name))
            {
                queryBuilder.Append(" AND Name LIKE @Name");
                totalCountQuery.Append(" AND Name LIKE @Name");
                parameters.Add("Name", $"%{filterParams.Name}%");
            }

            //Filter by Price
            if (filterParams?.Price.HasValue == true)
            {
                queryBuilder.Append(" AND Price = @Price");
                totalCountQuery.Append(" AND Price = @Price");
                parameters.Add("Price", $"{filterParams.Price}");
            }

            //Filter by Category
            if (filterParams?.Category.HasValue == true)
            {
                queryBuilder.Append(" AND Category = @Category");
                totalCountQuery.Append(" AND Category = @Category");
                parameters.Add("Category", $"{(int)filterParams.Category}");
            }

            //Filter by Description
            if (!string.IsNullOrEmpty(filterParams?.Description))
            {
                queryBuilder.Append(" AND Description LIKE @Description");
                totalCountQuery.Append(" AND Description LIKE @Description");
                parameters.Add("Description", $"%{filterParams.Description}%");
            }

            //Filter by ContentType
            if (filterParams?.ContentType.HasValue == true)
            {
                queryBuilder.Append(" AND ContentType = @ContentType");
                totalCountQuery.Append(" AND ContentType = @ContentType");
                parameters.Add("ContentType", $"{(int)filterParams.ContentType}");
            }

            //Filter by Unit
            if (filterParams?.Unit.HasValue == true)
            {
                queryBuilder.Append(" AND Unit = @Unit");
                totalCountQuery.Append(" AND Unit = @Unit");
                parameters.Add("Unit", $"{filterParams.Unit}");
            }

            //Filter by UploadType
            if (filterParams?.UploadType.HasValue == true)
            {
                queryBuilder.Append(" AND UploadType = @UploadType");
                totalCountQuery.Append(" AND UploadType = @UploadType");
                parameters.Add("UploadType", $"{(int)filterParams.UploadType}");
            }

            //Filter by TotalPage
            if (filterParams?.TotalPage.HasValue == true)
            {
                queryBuilder.Append(" AND TotalPage = @TotalPage");
                totalCountQuery.Append(" AND TotalPage = @TotalPage");
                parameters.Add("TotalPage", $"{filterParams.TotalPage}");
            }

            //Filter by Size
            if (filterParams?.Size.HasValue == true)
            {
                queryBuilder.Append(" AND Size = @Size");
                totalCountQuery.Append(" AND Size = @Size");
                parameters.Add("Size", $"{filterParams.Size}");
            }

            //Filter by Rating
            if (filterParams?.Rating.HasValue == true)
            {
                queryBuilder.Append(" AND Rating = @Rating");
                totalCountQuery.Append(" AND Rating = @Rating");
                parameters.Add("Rating", $"{filterParams.Rating}");
            }

            //Filter by BookId
            if (filterParams?.BookId.HasValue == true)
            {
                queryBuilder.Append(" AND BookId = @BookId");
                totalCountQuery.Append(" AND BookId = @BookId");
                parameters.Add("BookId", $"{filterParams.BookId}");
            }

            //Count TotalCount
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountQuery.ToString(), parameters);

            //Count TotalPages
            var totalPages = Math.Ceiling((totalCount / (double)pageSize));

            var offset = (pageIndex - 1) * pageSize;
            var paginatedQuery = $"{queryBuilder} ORDER BY p.Id OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            var items = (await connection.QueryAsync<Product>(paginatedQuery, parameters)).ToList();

            return new PagedResult<Product>(items, pageIndex, pageSize, totalCount, totalPages);
        }
    }

    public async Task<IEnumerable<Product>> GetProductsInCartByListIdsAsync(Guid accountId, List<Guid> productIds)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            // Valid columns for selecting
            var validColumns = new HashSet<string> { "p.Id", "p.Name", "Price", "p.Category", "p.Unit", "p.ContentType", "p.UploadType", "p.TotalPage", "p.Size", "p.ImageUrl", "p.FileUrl", "p.FileDemoUrl", "p.Rating", "p.IsPublic", "p.IsApproved" };

            // If no selected columns, select all
            var selectedColumnsString = string.Join(", ", validColumns); ;

            // Start building the query
            var queryBuilder = new StringBuilder(
                $@"SELECT {selectedColumnsString} FROM Products p 
                JOIN Carts c ON p.Id = c.ProductId
                WHERE 1=1 AND p.IsDeleted = 0 AND c.AccountId = @AccountId AND p.Id IN @ProductIds");

            var parameters = new DynamicParameters();

            parameters.Add("AccountId", $"{accountId}");
            parameters.Add("ProductIds", productIds);


            var items = (await connection.QueryAsync<Product>(queryBuilder.ToString(), parameters)).ToList();

            return items;
        }
    }

    public async Task<PagedResult<Product>> GetProductsPurchasedAsync(int pageIndex, int pageSize, Filter.ProductFilter filterParams, string[] selectedColumns)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            // Valid columns for selecting
            var validColumns = new HashSet<string> { "p.Id", "p.Name", "Price", "p.Category", "p.Unit", "p.ContentType", "p.UploadType", "p.TotalPage", "p.Size", "p.ImageUrl", "p.FileUrl", "p.FileDemoUrl", "p.Rating", "p.IsPublic", "p.IsApproved" };
            var columns = selectedColumns?.Where(c => validColumns.Contains(c)).ToArray();

            // If no selected columns, select all
            var selectedColumnsString = columns?.Length > 0 ? string.Join(", ", columns) : string.Join(", ", validColumns); ;

            // Start building the query
            var queryBuilder = new StringBuilder(
                $@"SELECT {selectedColumnsString} FROM Products p 
                JOIN OrderDetails od ON p.Id = od.ProductId
                JOIN Orders o ON o.Id = od.OrderId                
                WHERE 1=1 AND p.IsDeleted = 0 AND o.AccountId = @AccountId");

            var parameters = new DynamicParameters();

            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize > 100 ? 100 : pageSize;

            var totalCountQuery = new StringBuilder($@"
                SELECT COUNT(1) 
                FROM Products p
                JOIN OrderDetails od ON p.Id = od.ProductId
                JOIN Orders o ON o.Id = od.OrderId
                WHERE 1=1 AND p.IsDeleted = 0 AND o.AccountId = @AccountId");

            parameters.Add("AccountId", $"{filterParams.UserId}");

            // Filter by Name
            if (!string.IsNullOrEmpty(filterParams?.Name))
            {
                queryBuilder.Append(" AND Name LIKE @Name");
                totalCountQuery.Append(" AND Name LIKE @Name");
                parameters.Add("Name", $"%{filterParams.Name}%");
            }

            //Filter by Price
            if (filterParams?.Price.HasValue == true)
            {
                queryBuilder.Append(" AND Price = @Price");
                totalCountQuery.Append(" AND Price = @Price");
                parameters.Add("Price", $"{filterParams.Price}");
            }

            //Filter by Category
            if (filterParams?.Category.HasValue == true)
            {
                queryBuilder.Append(" AND Category = @Category");
                totalCountQuery.Append(" AND Category = @Category");
                parameters.Add("Category", $"{(int)filterParams.Category}");
            }

            //Filter by Description
            if (!string.IsNullOrEmpty(filterParams?.Description))
            {
                queryBuilder.Append(" AND Description LIKE @Description");
                totalCountQuery.Append(" AND Description LIKE @Description");
                parameters.Add("Description", $"%{filterParams.Description}%");
            }

            //Filter by ContentType
            if (filterParams?.ContentType.HasValue == true)
            {
                queryBuilder.Append(" AND ContentType = @ContentType");
                totalCountQuery.Append(" AND ContentType = @ContentType");
                parameters.Add("ContentType", $"{(int)filterParams.ContentType}");
            }

            //Filter by Unit
            if (filterParams?.Unit.HasValue == true)
            {
                queryBuilder.Append(" AND Unit = @Unit");
                totalCountQuery.Append(" AND Unit = @Unit");
                parameters.Add("Unit", $"{filterParams.Unit}");
            }

            //Filter by UploadType
            if (filterParams?.UploadType.HasValue == true)
            {
                queryBuilder.Append(" AND UploadType = @UploadType");
                totalCountQuery.Append(" AND UploadType = @UploadType");
                parameters.Add("UploadType", $"{(int)filterParams.UploadType}");
            }

            //Filter by TotalPage
            if (filterParams?.TotalPage.HasValue == true)
            {
                queryBuilder.Append(" AND TotalPage = @TotalPage");
                totalCountQuery.Append(" AND TotalPage = @TotalPage");
                parameters.Add("TotalPage", $"{filterParams.TotalPage}");
            }

            //Filter by Size
            if (filterParams?.Size.HasValue == true)
            {
                queryBuilder.Append(" AND Size = @Size");
                totalCountQuery.Append(" AND Size = @Size");
                parameters.Add("Size", $"{filterParams.Size}");
            }

            //Filter by Rating
            if (filterParams?.Rating.HasValue == true)
            {
                queryBuilder.Append(" AND Rating = @Rating");
                totalCountQuery.Append(" AND Rating = @Rating");
                parameters.Add("Rating", $"{filterParams.Rating}");
            }

            //Filter by BookId
            if (filterParams?.BookId.HasValue == true)
            {
                queryBuilder.Append(" AND BookId = @BookId");
                totalCountQuery.Append(" AND BookId = @BookId");
                parameters.Add("BookId", $"{filterParams.BookId}");
            }

            //Count TotalCount
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountQuery.ToString(), parameters);

            //Count TotalPages
            var totalPages = Math.Ceiling((totalCount / (double)pageSize));

            var offset = (pageIndex - 1) * pageSize;
            var paginatedQuery = $"{queryBuilder} ORDER BY p.Id OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            var items = (await connection.QueryAsync<Product>(paginatedQuery, parameters)).ToList();

            return new PagedResult<Product>(items, pageIndex, pageSize, totalCount, totalPages);
        }
    }

    public async Task<bool> IsProductPurchasedByUserAsync(Guid productId, Guid accountId)
    {
        var sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM Products p JOIN OrderDetails od ON p.Id = od.ProductId JOIN Orders o ON o.Id = od.OrderId WHERE p.Id = @Id AND o.AccountId = @AccountId) THEN 1 ELSE 0 END";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteScalarAsync<bool>(sql, new { Id = productId, AccountId = accountId });
            return result;
        }
    }

    public Task<int> UpdateAsync(Product entity)
    {
        throw new NotImplementedException();
    }
}
