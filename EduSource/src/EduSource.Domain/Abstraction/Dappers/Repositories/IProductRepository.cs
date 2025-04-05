using EduSource.Contract.Abstractions.Shared;
using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using static EduSource.Contract.Services.Products.Filter;

namespace EduSource.Domain.Abstraction.Dappers.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<PagedResult<Product>> GetPagedAsync(int pageIndex, int pageSize, ProductFilter filterParams, string[] selectedColumns);
    Task<PagedResult<Product>> GetPagedByUserAsync(int pageIndex, int pageSize, ProductFilter filterParams, string[] selectedColumns);

    Task<PagedResult<Product>> GetProductsInCartAsync(int pageIndex, int pageSize, ProductFilter filterParams, string[] selectedColumns);

    Task<PagedResult<Product>> GetProductsPurchasedAsync(int pageIndex, int pageSize, ProductFilter filterParams, string[] selectedColumns);

    Task<IEnumerable<Product>> GetProductsInCartByListIdsAsync(Guid accountId, List<Guid> productIds);
    Task<bool> IsProductPurchasedByUserAsync(Guid productId, Guid accountId);

    Task<Product> GetDetailsAsync(Guid productId);
    Task<Product> GetDetailsByUserAsync(Guid productId, Guid AccountId);


}
