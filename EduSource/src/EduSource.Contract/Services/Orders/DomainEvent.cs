using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.DTOs.OrderDTOs;

namespace EduSource.Contract.Services.Orders;

public static class DomainEvent
{
    public record NotiUserOrderSuccess(Guid Id, string Email, string InvoiceNumber, string InvoiceDate, List<EmailOrderDTO> InvoiceItems, int TotalAmount) : IDomainEvent;
}
