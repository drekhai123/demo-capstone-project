﻿using MediatR;
using Newtonsoft.Json;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Services.Authentications;
using static EduSource.Domain.Exceptions.AuthenticationException;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Contract.Abstractions.Shared;

namespace EduSource.Application.UseCases.V2.Commands.Authentications;

public sealed class ForgotPasswordChangeCommandHandler : ICommandHandler<Command.ForgotPasswordChangeCommand>
{
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IPublisher _publisher;
    private readonly IResponseCacheService _responseCacheService;
    public ForgotPasswordChangeCommandHandler
        (IEFUnitOfWork efUnitOfWork,
        IPasswordHashService passwordHashService,
        IPublisher publisher,
        IResponseCacheService responseCacheService)
    {
        _efUnitOfWork = efUnitOfWork;
        _passwordHashService = passwordHashService;
        _publisher = publisher;
        _responseCacheService = responseCacheService;
    }
    /// <summary>
    /// Change password
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ErrorChangePasswordException"></exception>
    public async Task<Result> Handle(Command.ForgotPasswordChangeCommand request, CancellationToken cancellationToken)
    {
        // Get otp from previous step
        var forgotPasswordMemory = await _responseCacheService.GetCacheResponseAsync($"passwordchange_{request.Email}");
        string unescapedJson = JsonConvert.DeserializeObject<string>(forgotPasswordMemory);
        var otp = JsonConvert.DeserializeObject<string>(unescapedJson);

        // Check if the otp created from the previous step matches the otp sent by the client
        if (otp != request.Otp) throw new ErrorChangePasswordException();

        // Update account
        var account = await _efUnitOfWork.AccountRepository.GetAccountByEmailAsync(request.Email);
        // If account haven't system => Exception
        if (account == null) throw new EmailNotFoundException();

        var newPassword = _passwordHashService.HashPassword(request.Password);
        account.UpdatePassword(newPassword);
        _efUnitOfWork.AccountRepository.Update(account);
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);

        // Send email
        await Task.WhenAll(
           _publisher.Publish(new DomainEvent.UserPasswordChanged(Guid.NewGuid(), request.Email), cancellationToken)
       );

        await _responseCacheService.DeleteCacheResponseAsync($"passwordchange_{request.Email}");

        return Result.Success(new Success(MessagesList.AuthForgotPasswordChangeSuccess.GetMessage().Code,
            MessagesList.AuthForgotPasswordChangeSuccess.GetMessage().Message));
    }
}
