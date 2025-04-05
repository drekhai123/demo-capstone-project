namespace EduSource.Contract.DTOs.OrderDTOs;

public class CreateOrderBankingDTO
{
    public List<Guid> ProductIds { get; set; }
    public bool IsFromCart { get; set; }
}
