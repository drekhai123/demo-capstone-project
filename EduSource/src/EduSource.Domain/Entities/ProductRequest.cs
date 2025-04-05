using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Services.Orders;
using EduSource.Domain.Abstraction.Entities;

namespace EduSource.Domain.Entities;

public class ProductRequest : DomainEntity<int>
{
    public ProductRequest() { }
    public string Name { get; private set; }
    public int Price { get; private set; }
    public Guid AccountId { get; private set; }
    public virtual Account Account { get; private set; }   

    public virtual ICollection<OrderDetails> OrderDetails { get; private set; }

    public static ProductRequest CreateProductRequest(int id, string name, int price, Guid accountId)
    {
        return new ProductRequest()
        {
            Id = id,
            Name = name,
            Price = price,
            AccountId = accountId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsDeleted = false,
        };
    }
}
