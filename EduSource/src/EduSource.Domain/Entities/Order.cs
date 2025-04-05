using EduSource.Domain.Abstraction.Entities;

namespace EduSource.Domain.Entities;

public class Order : DomainEntity<Guid>
{
    public Order()
    {

    }

    public int TotalPrice { get; private set; }
    public long OrderCode { get; private set; }
    public string Description { get; private set; }
    public Guid AccountId { get; private set; }
    public virtual Account Account { get; private set; }
    public virtual ICollection<OrderDetails> OrderDetails { get; private set; }

    public static Order CreateOrder(Guid id, int totalPrice, long orderCode, string description, Guid accountId)
    {
        return new Order()
        {
            Id = id,
            TotalPrice = totalPrice,
            OrderCode = orderCode,
            Description = description,
            AccountId = accountId,
            IsDeleted = false
        };
    }

    public void UpdateAccount(Account account)
    {
        Account = account;
    }

    public void UpdateOrderDetails(List<OrderDetails> orderDetails)
    {
        OrderDetails = orderDetails;
    }
}
