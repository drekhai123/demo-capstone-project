using EduSource.Domain.Abstraction.Entities;

namespace EduSource.Domain.Entities;

public class Cart : DomainEntity<Guid>
{
    public Cart()
    {

    }
    public Guid AccountId { get; private set; }
    public virtual Account Account { get; private set; }
    public Guid? ComboId { get; private set; }
    public virtual Combo? Combo { get; private set; }
    public Guid? ProductId { get; private set; }
    public virtual Product? Product { get; private set; }

    public static Cart AddProductToCart(Guid accountId, Guid productId)
    {
        return new Cart()
        {
            AccountId = accountId,
            ProductId = productId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsDeleted = false,
        };
    }
}
