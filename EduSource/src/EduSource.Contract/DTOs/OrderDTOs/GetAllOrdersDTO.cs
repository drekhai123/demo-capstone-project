using EduSource.Contract.Enumarations.Order;

namespace EduSource.Contract.DTOs.OrderDTOs;

public static class GetAllOrdersDTO
{
    public sealed class GetAllOrdersRequestDTO
    {
        public SortType? SortType { get; set; }
        public bool? IsSortASC { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string? Description { get; set; }
    }
}
