﻿using Asp.Versioning;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduSource.Contract.Services.Authentications;
using EduSource.Contract.DTOs.AuthenticationDTOs;
using static EduSource.Domain.Exceptions.AuthenticationException;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EduSource.Contract.Enumarations.MessagesList;

namespace EduSource.Presentation.Controller.V1;

[ApiVersion(1)]
public class AuthenticationController : ApiController
{
    public AuthenticationController(ISender sender) : base(sender)
    { }

    [HttpPost("register", Name = "Register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> Register([FromBody] Command.RegisterCommand commands)
    {
        var result = await Sender.Send(commands);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost("verify-email", Name = "VerifyEmail")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> VerifyEmail([FromBody] Command.VerifyEmailCommand commands)
    {
        var result = await Sender.Send(commands);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost("login", Name = "Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> Login([FromBody] Query.LoginQuery Login)
    {
        var result = await Sender.Send(Login);
        if (result.IsFailure)
            return HandlerFailure(result);

        var value = result.Value;

        Response.Cookies.Append("refreshToken", value.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.Now.AddMinutes(131400),
        });

        var authProfileDTO = new AuthProfileDTO()
        {
            UserId = value.UserId,
            FirstName = value.FirstName,
            LastName = value.LastName,
            CropAvatarLink = value.CropAvatarLink,
            FullAvatarLink = value.FullAvatarLink,
            RoleId = value.RoleId,
        };

        var tokenDto = new TokenDTO()
        {
            AccessToken = value.AccessToken,
            TokenType = "Bearer"
        };

        return Ok(new
        {
            AuthProfile = authProfileDTO,
            Token = tokenDto,
        });
    }

    [HttpPost("login-google", Name = "LoginGoogleCommand")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LoginGoogle([FromBody] Command.LoginGoogleCommand LoginGoogle)
    {
        var result = await Sender.Send(LoginGoogle);
        if (result.IsFailure)
            return HandlerFailure(result);

        var value = result.Value;

        Response.Cookies.Append("refreshToken", value.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.Now.AddMinutes(131400),
        });

        var authProfileDTO = new AuthProfileDTO()
        {
            UserId = value.UserId,
            FirstName = value.FirstName,
            LastName = value.LastName,
            CropAvatarLink = value.CropAvatarLink,
            FullAvatarLink = value.FullAvatarLink,
            RoleId = value.RoleId,
        };

        var tokenDto = new TokenDTO()
        {
            AccessToken = value.AccessToken,
            TokenType = "Bearer"
        };

        return Ok(new
        {
            AuthProfile = authProfileDTO,
            Token = tokenDto,
        });
    }

    [HttpGet("refresh-token", Name = "RefreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null) throw new RefreshTokenNullException();

        var result = await Sender.Send(new Query.RefreshTokenQuery(refreshToken));
        if (result.IsFailure)
            return HandlerFailure(result);

        var value = result.Value;

        Response.Cookies.Append("refreshToken", value.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.Now.AddMinutes(131400),
        });

        return Ok(new
        {
            TokenType = "Bearer",
            value.AccessToken,
        });
    }

    [HttpPost("forgot-password-email", Name = "ForgotPasswordEmailCommand")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPasswordEmail([FromBody] Command.ForgotPasswordEmailCommand ForgotPasswordEmail)
    {
        var result = await Sender.Send(ForgotPasswordEmail);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost("forgot-password-otp", Name = "ForgotPasswordOtpCommand")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPasswordOtp([FromBody] Command.ForgotPasswordOtpCommand ForgotPasswordOtp)
    {
        var result = await Sender.Send(ForgotPasswordOtp);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost("forgot-password-change", Name = "ForgotPasswordChangeCommand")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPasswordChange([FromBody] Command.ForgotPasswordChangeCommand ForgotPasswordChange)
    {
        var result = await Sender.Send(ForgotPasswordChange);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout", Name = "Logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Logout()
    {
        var user = User.FindFirstValue("UserId");
        Response.Cookies.Delete("refreshToken");
        return Ok(Result.Success(new Success(MessagesList.AuthLogoutSuccess.GetMessage().Code,
            MessagesList.AuthLogoutSuccess.GetMessage().Message)));
    }
}