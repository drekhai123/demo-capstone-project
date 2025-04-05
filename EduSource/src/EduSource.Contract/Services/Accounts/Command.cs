using Microsoft.AspNetCore.Http;
using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.DTOs.Account;
using EduSource.Contract.Enumarations.Authentication;

namespace EduSource.Contract.Services.Accounts;

public static class Command
{
    public record UpdateInfoCommand(Guid UserId, string FirstName, string LastName, string PhoneNumber, GenderType Gender) : ICommand<Success<Response.UserResponse>>;
    public record UpdateAvatarCommand(Guid UserId, IFormFile CropAvatarFile, IFormFile FullAvatarFile) : ICommand<Success<AccountAvatarDto>>;
    public record UpdateEmailCommand(Guid UserId, string Email) : ICommand<Success>;
    public record VerifyUpdateEmailCommand(Guid UserId) : ICommand<Success>;
    public record ChangePasswordCommand(Guid UserId, string Password) : ICommand<Success>;
    public record VerifyChangePasswordCommand(Guid UserId) : ICommand<Success>;
}
