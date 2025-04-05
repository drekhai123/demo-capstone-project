using EduSource.Contract.Enumarations.Authentication;

namespace EduSource.Domain.Entities;

public class Role
{
    public Role()
    { }

    public RoleType Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public virtual ICollection<Account> Accounts { get; set; }
}
