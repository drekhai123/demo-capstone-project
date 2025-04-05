using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using static EduSource.Contract.Services.Products.Filter;
using static EduSource.Contract.Services.Products.Response;

namespace EduSource.Contract.Services.Carts;

public static class Query
{
    public record GetAllProductsFromCartQuery(int PageIndex,
                int PageSize,
                ProductFilter FilterParams,
                string[] SelectedColumns) : IQuery<Success<PagedResult<ProductResponse>>>;
}
