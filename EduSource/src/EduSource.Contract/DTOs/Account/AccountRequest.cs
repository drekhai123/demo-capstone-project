using Microsoft.AspNetCore.Http;
using EduSource.Contract.Enumarations.Authentication;

namespace EduSource.Contract.DTOs.Account;

public static class AccountRequest
{
    public record UpdateAvatarRequestDto(IFormFile CropAvatar, IFormFile FullAvatar);
    public record UpdateInfoProfileRequestDto(string FirstName, string LastName, string PhoneNumber, GenderType Gender);
    public record UpdateEmailRequestDto(string Email);
    public record ChangePasswordRequestDto(string Password);
}
