using EduSource.Domain.Abstraction.Entities;

namespace EduSource.Domain.Entities;

public class ProductInCombo : DomainEntity<Guid>
{
    public ProductInCombo()
    {

    }
    public Guid ComboId { get; private set; }
    public virtual Combo Combo { get; private set; }
    public Guid? ProductId { get; private set; }
    public virtual Product? Product { get; private set; }   
}
