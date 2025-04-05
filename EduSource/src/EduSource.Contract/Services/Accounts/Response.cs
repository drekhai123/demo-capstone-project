using EduSource.Contract.DTOs.Account;
using EduSource.Contract.Enumarations.Authentication;
using System.Security.Principal;

namespace EduSource.Contract.Services.Accounts;
public static class Response
{
    public record UserResponse(Guid Id, string FirstName, string LastName, string Email, string PhoneNumber, GenderType Gender, LoginType? LoginType = LoginType.Local);

    public record UserInfoResponse(Guid Id, string FirstName, string LastName, string Email, string PhoneNumber, GenderType Gender, LoginType LoginType, string? CropAvatarUrl, string? FullAvatarUrl, string? CropCoverPhotoUrl, string? FullCoverPhotoUrl, string? Biography);
    public record UsersResponse(Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool IsDeleted,
    LoginType LoginType,
    GenderType Gender,
    string? CropAvatarUrl,
    string? CropAvatarId,
    string? FullAvatarUrl,
    string? FullAvatarId,
    DateTime? CreatedAt);    
}
