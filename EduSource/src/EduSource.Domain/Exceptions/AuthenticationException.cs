using EduSource.Contract.Enumarations.MessagesList;
namespace EduSource.Domain.Exceptions;

public static class AuthenticationException
{
    public sealed class EmailExistException : BadRequestException
    {
        public EmailExistException()
            : base(MessagesList.AuthEmailExistException.GetMessage().Message,
                   MessagesList.AuthEmailExistException.GetMessage().Code)
        { }
    }

    public sealed class RegisterTimeOutException : BadRequestException
    {
        public RegisterTimeOutException()
            : base(MessagesList.AuthRegisterTimeOutException.GetMessage().Message,
                   MessagesList.AuthRegisterTimeOutException.GetMessage().Code)
        { }
    }

    public sealed class OtpForgotPasswordNotMatchException : NotFoundException
    {
        public OtpForgotPasswordNotMatchException()
            : base(MessagesList.AuthOtpForgotPasswordNotMatchException.GetMessage().Message,
                   MessagesList.AuthOtpForgotPasswordNotMatchException.GetMessage().Code)
        { }
    }

    public sealed class EmailNotFoundException : NotFoundException
    {
        public EmailNotFoundException()
           : base(MessagesList.AuthEmailNotFoundException.GetMessage().Message,
                  MessagesList.AuthEmailNotFoundException.GetMessage().Code)
        { }
    }

    public sealed class ErrorChangePasswordException : BadRequestException
    {
        public ErrorChangePasswordException()
           : base(MessagesList.AuthErrorChangePasswordException.GetMessage().Message,
                  MessagesList.AuthErrorChangePasswordException.GetMessage().Code)
        { }
    }

    public sealed class EmailGoogleRegistedException : BadRequestException
    {
        public EmailGoogleRegistedException()
           : base(MessagesList.AuthGoogleEmailRegisted.GetMessage().Message,
                  MessagesList.AuthGoogleEmailRegisted.GetMessage().Code)
        { }
    }

    public sealed class AccountRegisteredAnotherMethodException : BadRequestException
    {
        public AccountRegisteredAnotherMethodException()
           : base(MessagesList.AuthAccountRegisteredAnotherMethod.GetMessage().Message,
                  MessagesList.AuthAccountRegisteredAnotherMethod.GetMessage().Code)
        { }
    }

    public sealed class AccountBanned : BadRequestException
    {
        public AccountBanned()
           : base(MessagesList.AccountBanned.GetMessage().Message,
                  MessagesList.AccountBanned.GetMessage().Code)
        { }
    }

    public sealed class PasswordNotMatchException : BadRequestException
    {
        public PasswordNotMatchException()
            : base(MessagesList.AuthPasswordNotMatchException.GetMessage().Message,
                   MessagesList.AuthPasswordNotMatchException.GetMessage().Code)
        { }
    }

    public sealed class RefreshTokenNullException : AuthorizeException
    {
        public RefreshTokenNullException()
           : base(MessagesList.AuthRefreshTokenNull.GetMessage().Message,
                  MessagesList.AuthRefreshTokenNull.GetMessage().Code)
        { }
    }

    public sealed class LoginGoogleFailException : BadRequestException
    {
        public LoginGoogleFailException()
           : base(MessagesList.AuthLoginGoogleFail.GetMessage().Message,
                  MessagesList.AuthLoginGoogleFail.GetMessage().Code)
        { }
    }
}
