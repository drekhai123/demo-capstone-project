using EduSource.Domain.Abstraction.Entities;

namespace EduSource.Domain.Entities;

public class Feedback : DomainEntity<Guid>
{
    public Feedback()
    {

    }

    public string Content { get; private set; }
    public double Rating { get; private set; }
    public Guid AccountId { get; private set; }
    public virtual Account Account { get; private set; }
    public Guid? ComboId { get; private set; }
    public virtual Combo? Combo { get; private set; }
    public Guid? ProductId { get; private set; }
    public virtual Product? Product { get; private set; }
}
