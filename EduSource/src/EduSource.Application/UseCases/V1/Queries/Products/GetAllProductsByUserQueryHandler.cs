using AutoMapper;
using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Products;
using EduSource.Domain.Abstraction.Dappers;
using System.Collections;
using static EduSource.Contract.Services.Books.Response;
using static EduSource.Contract.Services.Products.Response;

namespace EduSource.Application.UseCases.V1.Queries.Products;

public sealed class GetAllProductsByUserQueryHandler : IQueryHandler<Query.GetAllProductsByUserQuery, Success<PagedResult<ProductResponse>>>
{
    private readonly IDPUnitOfWork _dpUnitOfWork;
    public GetAllProductsByUserQueryHandler(IDPUnitOfWork dpUnitOfWork)
    {
        _dpUnitOfWork = dpUnitOfWork;
    }

    public async Task<Result<Success<PagedResult<ProductResponse>>>> Handle(Query.GetAllProductsByUserQuery request, CancellationToken cancellationToken)
    {
        // Find all Products
        var listProducts = await _dpUnitOfWork.ProductRepositories.GetPagedByUserAsync(request.PageIndex, request.PageSize, request.FilterParams, request.SelectedColumns);
        // Find all Orders of User
        var listOrdersByUser = await _dpUnitOfWork.OrderRepositories.GetAllOrdersByUserAsync(request.FilterParams.UserId.Value);
        var listProductsMapped = new List<ProductResponse>();
        listProducts.Items.ForEach(product =>
        {
            //Check in List Order: If Product has OrderDetails has OrderId == orderId of order in List Order)
            var isPurchased = false;
            if (product.OrderDetails.ToList().Count != 0)
            {
                isPurchased = listOrdersByUser.Any(order => order.Id == product.OrderDetails.ToList()[0].OrderId);
            }
            var fileUrl = isPurchased ? product.FileUrl : product.FileDemoUrl;
            listProductsMapped.Add(new ProductResponse(product.Id, product.Name, product.Price, product.Category, product.Unit, product.Description, product.ContentType, product.UploadType, product.TotalPage, product.Size, product.ImageUrl, fileUrl, product.Rating, product.IsPublic, product.IsApproved, null, null, isPurchased));
        });
        //Mapping Category to CategoryResponse
        var result = new PagedResult<ProductResponse>(listProductsMapped, listProducts.PageIndex, listProducts.PageSize, listProducts.TotalCount, listProducts.TotalPages);
        //Check if ListCategory is empty
        if (listProducts.Items.Count == 0)
        {
            return Result.Success(new Success<PagedResult<ProductResponse>>(MessagesList.ProductsNotFoundException.GetMessage().Code, MessagesList.ProductsNotFoundException.GetMessage().Message, result));
        }
        //Return result
        return Result.Success(new Success<PagedResult<ProductResponse>>(MessagesList.ProductGetAllProductsSuccess.GetMessage().Code, MessagesList.ProductGetAllProductsSuccess.GetMessage().Message, result));
    }
}
