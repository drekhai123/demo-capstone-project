using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.Authentication;
using MediatR;

namespace EduSource.Contract.Services.Authentications;
public static class Command
{
    public record RegisterCommand
        (string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Password,
        GenderType Gender)
        : ICommand;

    public record VerifyEmailCommand(string Email) : ICommand;
    public record LoginGoogleCommand(string AccessTokenGoogle) : ICommand<Response.LoginResponse>;
    public record ForgotPasswordEmailCommand(string Email) : ICommand;
    public record ForgotPasswordOtpCommand(string Email, string Otp) : ICommand;
    public record ForgotPasswordChangeCommand(string Email, string Password, string Otp) : ICommand;

}
