using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using static EduSource.Contract.Services.Orders.Filter;

namespace EduSource.Contract.Services.Orders;

public static class Query
{
    public record GetAllOrdersQuery(int PageIndex,
        int PageSize,
        OrderFilter FilterParams,
        string[] SelectedColumns) : IQuery<Success<PagedResult<Response.OrderResponse>>>;

    public record GetDashboardQuery(int year, int? month, int? week) : IQuery<Success<Response.DashboardResponse>>;

    //public record GetOrderByIdQuery(Guid Id) : IQuery<Success<Response.OrderResponse>>;
}
