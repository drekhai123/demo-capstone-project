using MediatR;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Services.Authentications;
using System.Security.Cryptography;
using System.Text.Json;
using EduSource.Domain.Abstraction.EntitiyFramework;
using static EduSource.Domain.Exceptions.AuthenticationException;
using EduSource.Contract.Enumarations.Authentication;
using EduSource.Contract.Enumarations.MessagesList;

namespace EduSource.Application.UseCases.V2.Commands.Authentications;

public sealed class ForgotPasswordEmailCommandHandler : ICommandHandler<Command.ForgotPasswordEmailCommand>
{
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IPublisher _publisher;
    private readonly IResponseCacheService _responseCacheService;

    public ForgotPasswordEmailCommandHandler
        (IEFUnitOfWork efUnitOfWork,
        IPublisher publisher,
        IResponseCacheService responseCacheService)
    {
        _efUnitOfWork = efUnitOfWork;
        _publisher = publisher;
        _responseCacheService = responseCacheService;
    }
    /// <summary>
    /// Send email to create otp
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="EmailNotFoundException"></exception>

    public async Task<Result> Handle(Command.ForgotPasswordEmailCommand request, CancellationToken cancellationToken)
    {
        // Check email is in system
        var userInfo = await _efUnitOfWork.AccountRepository.GetAccountByEmailAsync(request.Email);
        // If user haven't system => Exception
        if (userInfo == null) throw new EmailNotFoundException();
        // If user have type login != Local => Exception
        if (userInfo.LoginType != LoginType.Local)
            throw new EmailGoogleRegistedException();

        // Random OTP
        string otp = GenerateSecureOTP();

        // Save memory
        await _responseCacheService.SetCacheResponseAsync
            ($"forgotpassword_{request.Email}",
            JsonSerializer.Serialize(otp),
            TimeSpan.FromMinutes(15));

        // Send mail notification send otp
        await Task.WhenAll(
            _publisher.Publish(new DomainEvent.UserOtpChanged(Guid.NewGuid(), request.Email, otp), cancellationToken)
        );

        return Result.Success(new Success<string>(MessagesList.AuthForgotPasswordEmailSuccess.GetMessage().Code,
            MessagesList.AuthForgotPasswordEmailSuccess.GetMessage().Message, request.Email));
    }

    private static string GenerateSecureOTP()
    {
        var bytes = new byte[4];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        int otp = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 90000 + 10000;
        return otp.ToString();
    }
}
