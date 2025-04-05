using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Services.Authentications;
using EduSource.Contract.Settings;
using Microsoft.Extensions.Options;

namespace EduSource.Application.UseCases.V1.Events;

public sealed class SendEmailWhenUserChangedEventHandler 
    : IDomainEventHandler<DomainEvent.UserRegistedWithLocal>, 
    IDomainEventHandler<DomainEvent.UserVerifiedEmailRegist>,
    IDomainEventHandler<DomainEvent.UserOtpChanged>,
    IDomainEventHandler<DomainEvent.UserPasswordChanged>,
    IDomainEventHandler<DomainEvent.UserCreatedWithGoogle>,
    IDomainEventHandler<Contract.Services.Accounts.DomainEvent.UserEmailChanged>,
    IDomainEventHandler<Contract.Services.Accounts.DomainEvent.UserPasswordChanged>
{
    private readonly IEmailService _emailService;
    private readonly ClientSetting _clientSetting;

    public SendEmailWhenUserChangedEventHandler(IEmailService emailService,
        IOptions<ClientSetting> clientConfig)
    {
        _emailService = emailService;
        _clientSetting = clientConfig.Value;
    }

    public async Task Handle(DomainEvent.UserRegistedWithLocal notification, CancellationToken cancellationToken)
    {
        await _emailService.SendMailAsync
            (notification.Email,
            "Register EduSource",
            "EmailRegister.html", new Dictionary<string, string> {
            { "ToEmail", notification.Email},
            {"Link", $"{_clientSetting.Url}{_clientSetting.VerifyEmail}/{notification.Email}"}
        });
    }

    public async Task Handle(DomainEvent.UserVerifiedEmailRegist notification, CancellationToken cancellationToken)
    {
        await _emailService.SendMailAsync
           (notification.Email,
           "VerifyEmail EduSource",
           "EmailVerifyEmail.html", new Dictionary<string, string> {
            { "ToEmail", notification.Email},
       });
    }

    public async Task Handle(DomainEvent.UserOtpChanged notification, CancellationToken cancellationToken)
    {
        await _emailService.SendMailAsync
            (notification.Email,
            "Forgot password EduSource",
            "EmailForgotPassword.html", new Dictionary<string, string> {
            {"ToEmail", notification.Email},
            {"Otp", notification.Otp}
        });
    }

    public async Task Handle(DomainEvent.UserPasswordChanged notification, CancellationToken cancellationToken)
    {
        await _emailService.SendMailAsync
            (notification.Email,
            "Password changed",
            "EmailPasswordChanged.html", new Dictionary<string, string> {
            {"ToEmail", notification.Email},
        });
    }

    public async Task Handle(DomainEvent.UserCreatedWithGoogle notification, CancellationToken cancellationToken)
    {
        await _emailService.SendMailAsync
            (notification.Email,
            "Register with Google",
            "EmailRegisterWithGoogle.html", new Dictionary<string, string> {
            {"ToEmail", notification.Email},
        });
    }

    public async Task Handle(Contract.Services.Accounts.DomainEvent.UserEmailChanged notification, CancellationToken cancellationToken)
    {
        await _emailService.SendMailAsync
            (notification.Email,
            "Change email",
            "EmailUserChangeEmail.html", new Dictionary<string, string> {
            {"ToEmail", notification.Email},
               {"Link", $"{_clientSetting.Url}{_clientSetting.VerifyChangeEmail}/{notification.UserId}"}
        });
    }

    public async Task Handle(Contract.Services.Accounts.DomainEvent.UserPasswordChanged notification, CancellationToken cancellationToken)
    {
        await _emailService.SendMailAsync
           (notification.Email,
           "Change password",
           "EmailUserChangePassword.html", new Dictionary<string, string> {
            {"ToEmail", notification.Email},
               {"Link", $"{_clientSetting.Url}{_clientSetting.VerifyChangePassword}/{notification.UserId}"}
       });
    }
}
