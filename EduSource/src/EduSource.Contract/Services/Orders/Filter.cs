using EduSource.Contract.Enumarations.Order;

namespace EduSource.Contract.Services.Orders;

public static class Filter
{
    public record OrderFilter(SortType? SortType, bool? IsSortASC, int? MinValue, int? MaxValue, string? Description, Guid? AccountId);
}
