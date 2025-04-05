using EduSource.Contract.Abstractions.Shared;
using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using static EduSource.Contract.Services.Orders.Filter;

namespace EduSource.Domain.Abstraction.Dappers.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order>> GetAllOrdersByUserAsync(Guid AccountId);
    Task<PagedResult<Order>> GetAllOrdersByAdminAsync(int pageIndex, int pageSize, OrderFilter filterParams, string[] selectedColumns);
    Task<int> CountAllOrders();
    Task<int> GetTotalMoneyOfOrdersInMonth(int month, int year);
    Task<int> GetTotalMoneyOfOrdersInDay(DateTime date);

    Task<Dictionary<DateTime, (int TotalPrice, int OrdersCount)>> GetRevenueInListDates(List<DateTime> dates);
    Task<Dictionary<int, (int TotalPrice, int OrdersCount)>> GetRevenueInMonth(int year, int month);
    Task<Dictionary<DateTime, (int TotalPrice, int OrdersCount)>> GetRevenueInYear(int year);

}
