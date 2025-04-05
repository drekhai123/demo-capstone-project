using EduSource.Contract.DTOs.PaymentDTOs;

namespace EduSource.Contract.Abstractions.Services;

public interface IPaymentService
{
    Task<CreatePaymentResponseDTO> CreatePaymentLink(CreatePaymentDTO paymentData);
    Task<bool> CancelOrder(long orderId);
}
