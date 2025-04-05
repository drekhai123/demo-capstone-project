using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.Authentication;
using EduSource.Contract.Services.Authentications;
using EduSource.Domain.Abstraction.Dappers;
using EduSource.Domain.Abstraction.EntitiyFramework;
using MediatR;
using static EduSource.Domain.Exceptions.AuthenticationException;

namespace EduSource.Application.UseCases.V1.Commands.Authentications;

public sealed class LoginGoogleCommandHandler : ICommandHandler<Command.LoginGoogleCommand, Response.LoginResponse>
{
    private readonly IGoogleOAuthService _googleOAuthService;
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IPublisher _publisher;
    private readonly ITokenGeneratorService _tokenGeneratorService;

    public LoginGoogleCommandHandler(IGoogleOAuthService googleOAuthService, IEFUnitOfWork efUnitOfWork, IPublisher publisher, ITokenGeneratorService tokenGeneratorService)
    {
        _googleOAuthService = googleOAuthService;
        _efUnitOfWork = efUnitOfWork;
        _publisher = publisher;
        _tokenGeneratorService = tokenGeneratorService;
    }

    public async Task<Result<Response.LoginResponse>> Handle(Command.LoginGoogleCommand request, CancellationToken cancellationToken)
    {
        //Get User Information from access token Google
        var googleUserInfo = await _googleOAuthService.ValidateTokenAsync(request.AccessTokenGoogle);
        //Check if it does not exist any account from access token Google
        if (googleUserInfo == null)
        {
            throw new LoginGoogleFailException();
        }
        // Check email have exit
        var account = await _efUnitOfWork.AccountRepository.FindSingleAsync(acc => acc.Email == googleUserInfo.Email);
        // If have not account => Register account with type login Google
        if (account == null)
        {
            // Create object account member
            var accountMember = Domain.Entities.Account.CreateMemberAccountGoogle
                (googleUserInfo.Name, "", googleUserInfo.Email, GenderType.Male);
            _efUnitOfWork.AccountRepository.Add(accountMember);
            // Save account
            await _efUnitOfWork.SaveChangesAsync();
            // Send mail when created success
            await Task.WhenAll(
                _publisher.Publish(new DomainEvent.UserCreatedWithGoogle(Guid.NewGuid(), googleUserInfo.Email),
                cancellationToken)
            );

            // Generate accessToken and refreshToken
            var accessToken = _tokenGeneratorService.GenerateAccessToken(accountMember.Id, (int)accountMember.RoleId);
            var refrehsToken = _tokenGeneratorService.GenerateRefreshToken(accountMember.Id, (int)accountMember.RoleId);

            return Result.Success
                (new Response.LoginResponse
                (accountMember.Id,
                accountMember.FirstName,
                accountMember.LastName,
                accountMember.CropAvatarUrl,
                accountMember.FullAvatarUrl,
                (int)accountMember.RoleId,
                accessToken,
                refrehsToken));
        }
        else
        {
            // If have account, check account not type Google
            if (account.LoginType != LoginType.Google) throw new AccountRegisteredAnotherMethodException();

            // If account banned
            if (account.IsDeleted == true) throw new AccountBanned();

            // Generate accessToken and refreshToken
            var accessToken = _tokenGeneratorService.GenerateAccessToken(account.Id, (int)account.RoleId);
            var refrehsToken = _tokenGeneratorService.GenerateRefreshToken(account.Id, (int)account.RoleId);

            return Result.Success
                (new Response.LoginResponse
                (account.Id,
                account.FirstName,
                account.LastName,
                account.CropAvatarUrl,
                account.FullAvatarUrl,
                (int)account.RoleId,
                accessToken,
                refrehsToken));
        }
    }
}
