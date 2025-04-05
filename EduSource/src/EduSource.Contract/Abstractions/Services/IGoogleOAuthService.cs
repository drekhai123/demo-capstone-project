using EduSource.Contract.DTOs.AuthenticationDTOs;

namespace EduSource.Contract.Abstractions.Services;

public interface IGoogleOAuthService
{
    Task<GoogleUserInfoDTO> ValidateTokenAsync(string AccessTokenGoogle);
}
