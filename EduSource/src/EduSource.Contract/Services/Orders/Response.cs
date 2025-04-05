using static EduSource.Contract.DTOs.OrderDTOs.DashboardDTO;
using static EduSource.Contract.DTOs.OrderDTOs.OrderResponseDTO;

namespace EduSource.Contract.Services.Orders;

public static class Response
{
    //public record ProductResponse(Guid Id, string Name, StatusType StatusType, string Policies, string Description, double Rating, double Price, double Value, int MaximumRentDays, ConfirmStatus ConfirmStatus, bool IsAddedToWishlist, CategoryDTO Category, List<string> ProductImagesUrl, InsuranceDTO.InsuranceResponseDTO Insurance, List<SurchargeDTO.SurchargeResponseDTO> Surcharges, LessorDTO Lessor);
    public record OrderSuccess(string Url);
    public record OrderFail(string Url);
    public record OrderRequestSuccess(string Url);
    public record OrderRequestFail(string Url);
    public record OrderResponse(Guid Id, int TotalAmount, int PaymentAmount, DateTime PaidAt, string Description, long OrderCode, AccountResponseDTO Account, List<OrderDetailsResponseDTO> OrderDetails);
    //public record OrderResponse(Guid Id, DateTime RentTime, DateTime ReturnTime, string DeliveryAddress, double OrderValue, OrderStatusType OrderStatus, OrderReportStatusType OrderReportStatus, string? UserReasonReject, string? LessorReasonReject, string? UserReport, string? AdminReasonReject, DateTime CreatedDate, ProductResponseDTO Product, UserDTO User, LessorDTO Lessor);

    public record DashboardResponse(string Category, List<DataDTO> Data, MonthlyTargetDTO MonthlyTarget, int CustomersCount, int OrdersCount);
}
