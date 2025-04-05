using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.Authentication;
using EduSource.Contract.Services.Authentications;
using EduSource.Domain.Abstraction.Dappers;
using EduSource.Domain.Entities;
using EduSource.Domain.Exceptions;
using static EduSource.Domain.Exceptions.AuthenticationException;

namespace EduSource.Application.UseCases.V1.Queries.Authentications;

public sealed class LoginQueryHandler : IQueryHandler<Query.LoginQuery, Response.LoginResponse>
{
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly IDPUnitOfWork _dpUnitOfWork;
    private readonly IPasswordHashService _passwordHashService;

    public LoginQueryHandler(ITokenGeneratorService tokenGeneratorService, IDPUnitOfWork dpUnitOfWork, IPasswordHashService passwordHashService)
    {
        _tokenGeneratorService = tokenGeneratorService;
        _dpUnitOfWork = dpUnitOfWork;
        _passwordHashService = passwordHashService;
    }

    public async Task<Result<Response.LoginResponse>> Handle(Query.LoginQuery request, CancellationToken cancellationToken)
    {
        //Get account by email
        var accountFoundByEmail = await _dpUnitOfWork.AccountRepositories.GetByEmailAsync(request.Email);
        //Check if account is not found
        if(accountFoundByEmail == null)
        {
            throw new EmailNotFoundException();
        }
        //Check if account is not created in local type
        if (accountFoundByEmail.LoginType != LoginType.Local)
        {
            throw new AccountRegisteredAnotherMethodException();
        }
        //Check if account is banned
        if (accountFoundByEmail.IsDeleted == true) 
        { 
            throw new AccountBanned(); 
        }
        //Check if password does not equal with password hash
        var isVerifyPassword = _passwordHashService.VerifyPassword(request.Password, accountFoundByEmail.Password);
        if (isVerifyPassword == false) 
        { 
            throw new PasswordNotMatchException(); 
        }
        // Generate accessToken and refreshToken
        var accessToken = _tokenGeneratorService.GenerateAccessToken(accountFoundByEmail.Id, (int)accountFoundByEmail.RoleId);
        var refreshToken = _tokenGeneratorService.GenerateRefreshToken(accountFoundByEmail.Id, (int)accountFoundByEmail.RoleId);

        return Result.Success
            (new Response.LoginResponse
            (accountFoundByEmail.Id,
            accountFoundByEmail.FirstName,
            accountFoundByEmail.LastName,
            accountFoundByEmail.CropAvatarUrl,
            accountFoundByEmail.FullAvatarUrl,
            (int)accountFoundByEmail.RoleId,
            accessToken,
            refreshToken));
    }
}
