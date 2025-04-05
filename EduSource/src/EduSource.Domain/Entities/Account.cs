using EduSource.Contract.Enumarations.Authentication;
using EduSource.Domain.Abstraction.Entities;

namespace EduSource.Domain.Entities;

public class Account : DomainEntity<Guid>
{
    public Account()
    {
    }

    public Account
        (Guid id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        bool status,
        string password,
        GenderType gender,
        string cropAvatarUrl,
        string cropAvatarId,
        string fullAvatarUrl,
        string fullAvatarId,
        LoginType loginType,
        RoleType roleId
        )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Password = password;
        GenderType = gender;
        CropAvatarUrl = cropAvatarUrl;
        CropAvatarId = cropAvatarId;
        FullAvatarUrl = fullAvatarUrl;
        FullAvatarId = fullAvatarId;
        LoginType = loginType;
        RoleId = roleId;
        IsDeleted = false;
    }

    public Account
        (string firstName,
        string lastName,
        string email,
        string phoneNumber,
        bool status,
        string password,
        GenderType gender,
        string cropAvatarUrl,
        string cropAvatarId,
        string fullAvatarUrl,
        string fullAvatarId,
        LoginType loginType,
        RoleType roleId
        )
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Password = password;
        GenderType = gender;
        CropAvatarUrl = cropAvatarUrl;
        CropAvatarId = cropAvatarId;
        FullAvatarUrl = fullAvatarUrl;
        FullAvatarId = fullAvatarId;
        LoginType = loginType;
        RoleId = roleId;
        IsDeleted = false;
    }

    public Account(string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string password,
        string? cropAvatarUrl,
        string? cropAvatarId,
        string? fullAvatarUrl,
        string? fullAvatarId,
        string? cropCoverPhotoUrl,
        string? cropCoverPhotoId,
        string? fullCoverPhotoUrl,
        string? fullCoverPhotoId,
        string? biography,
        LoginType loginType,
        GenderType genderType,
        RoleType roleUserId,
        bool isDeleted)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Password = password;
        CropAvatarUrl = cropAvatarUrl;
        CropAvatarId = cropAvatarId;
        FullAvatarUrl = fullAvatarUrl;
        FullAvatarId = fullAvatarId;
        CropCoverPhotoUrl = cropAvatarUrl;
        CropCoverPhotoId = cropAvatarId;
        FullCoverPhotoUrl = fullAvatarUrl;
        FullCoverPhotoId = fullAvatarId;
        Biography = biography;
        LoginType = loginType;
        GenderType = genderType;
        RoleId = roleUserId;
        IsDeleted = isDeleted;
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string? CropAvatarUrl { get; private set; }
    public string? CropAvatarId { get; private set; }
    public string? FullAvatarUrl { get; private set; }
    public string? FullAvatarId { get; private set; }
    public string? CropCoverPhotoUrl { get; private set; }
    public string? CropCoverPhotoId { get; private set; }
    public string? FullCoverPhotoUrl { get; private set; }
    public string? FullCoverPhotoId { get; private set; }
    public string? Biography { get; private set; }
    public LoginType LoginType { get; private set; }
    public GenderType GenderType { get; private set; }
    public RoleType RoleId { get; private set; }
    public virtual Role Role { get; private set; }
    public virtual Seller Seller { get; private set; }
    public virtual ICollection<Cart> Carts { get; private set; }
    public virtual ICollection<Feedback> Feedbacks { get; private set; }
    public virtual ICollection<Order> Orders { get; private set; }
    public virtual ICollection<Combo> Combos { get; private set; }
    public virtual ICollection<Wishlist> Wishlists { get; private set; }
    public virtual ICollection<Product> Products { get; private set; }
    public virtual ICollection<ProductRequest> ProductRequests { get; private set; }



    public static Account CreateMemberAccountLocal
        (string firstName, string lastName, string email, string phoneNumber, string password, GenderType gender)
    {
        string avatarUrl = "https://res.cloudinary.com/dc4eascme/image/upload/v1735300380/edusource/male-avatar.png";
        if (gender == GenderType.Female)
        {
            avatarUrl = "https://res.cloudinary.com/dc4eascme/image/upload/v1735300380/edusource/female-avatar.png";
        }
        return new Account(firstName, lastName, email, phoneNumber, false, password, gender, avatarUrl, "", avatarUrl, "", LoginType.Local, RoleType.Member);
    }

    public static Account CreateMemberAccountGoogle
        (string firstName, string lastName, string email, GenderType gender)
    {
        string avatarUrl = "https://res.cloudinary.com/dc4eascme/image/upload/v1735300380/edusource/male-avatar.png";
        if (gender == GenderType.Female)
        {
            avatarUrl = "https://res.cloudinary.com/dc4eascme/image/upload/v1735300380/edusource/female-avatar.png";
        }
        return new Account(firstName, lastName, email, "", false, "", gender, avatarUrl, "", avatarUrl, "", LoginType.Google, RoleType.Member);
    }

    public static Account CreateAdminAccount
       (string email, string password)
    {
        string avatarUrl = "https://res.cloudinary.com/dc4eascme/image/upload/v1737386413/edusource/admin-avatar.png";
        return new Account("Admin", "", email, "", false, password, GenderType.Male, avatarUrl, "", avatarUrl, "", LoginType.Local, RoleType.Admin);
    }

    public static Account CreateStaffAssistant
      (Guid id, string email, string password)
    {
        string avatarUrl = "https://res.cloudinary.com/dc4eascme/image/upload/v1737386425/edusource/staff-avatar.png";
        return new Account(id, "Staff", "", email, "", false, password, GenderType.Male, avatarUrl, "", avatarUrl, "", LoginType.Local, RoleType.Staff);
    }

    public void UpdatePassword(string newPassword)
    {
        Password = newPassword;
    }

    public void UpdateAvatarProfileUser(string cropAvatarUrl, string cropAvatarId, string fullAvatarUrl, string fullAvatarId)
    {
        CropAvatarUrl = cropAvatarUrl;
        CropAvatarId = cropAvatarId;
        FullAvatarUrl = fullAvatarUrl;
        FullAvatarId = fullAvatarId;
    }

    public void UpdateInfoProfileUser(string firstName, string lastName, string phoneNumber, GenderType gender)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        GenderType = gender;
    }

    public void UpdateEmail(string email)
    {
        Email = email;
    }

    public void ChangeUserIsDelete(bool isDelete)
    {
        IsDeleted = isDelete;
    }
}
