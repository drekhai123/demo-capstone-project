using EduSource.Domain.Abstraction.Entities;

namespace EduSource.Domain.Entities;

public class Combo : DomainEntity<Guid>
{
    public Combo()
    {

    }

    public string Name { get; private set; }
    public double Price { get; private set; }
    public string Description { get; private set; }
    public string ImageId { get; private set; }
    public string ImageUrl { get; private set; }
    public bool IsApproved { get; private set; }
    public Guid AccountId { get; private set; }
    public virtual Account Account { get; private set; }
    public virtual ICollection<Cart> Carts { get; private set; }
    public virtual ICollection<Wishlist> Wishlists { get; private set; }
    public virtual ICollection<OrderDetails> OrderDetails { get; private set; }
    public virtual ICollection<ProductInCombo> ProductInCombos { get; private set; }
    public virtual ICollection<Feedback> Feedbacks { get; private set; }
}
