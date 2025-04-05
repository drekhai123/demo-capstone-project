using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using static EduSource.Contract.Services.Products.Filter;
using static EduSource.Contract.Services.Products.Response;

namespace EduSource.Contract.Services.Products;

public static class Query
{
    public record GetAllProductsQuery(int PageIndex,
            int PageSize,
            ProductFilter FilterParams,
            string[] SelectedColumns) : IQuery<Success<PagedResult<ProductResponse>>>;

    public record GetAllProductsByUserQuery(int PageIndex,
            int PageSize,
            ProductFilter FilterParams,
            string[] SelectedColumns) : IQuery<Success<PagedResult<ProductResponse>>>;

    public record GetAllProductsPurchasedQuery(int PageIndex,
            int PageSize,
            ProductFilter FilterParams,
            string[] SelectedColumns) : IQuery<Success<PagedResult<ProductResponse>>>;

    public record GetProductByIdQuery(Guid Id) : IQuery<Success<ProductResponse>>;
    public record GetProductByIdByUserQuery(Guid Id, Guid AccountId) : IQuery<Success<ProductResponse>>;

}
