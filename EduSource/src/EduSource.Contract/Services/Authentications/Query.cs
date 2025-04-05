using EduSource.Contract.Abstractions.Message;

namespace EduSource.Contract.Services.Authentications;

public static class Query
{
    public record LoginQuery(string Email, string Password) : IQuery<Response.LoginResponse>;
    public record RefreshTokenQuery
     (string Token) : IQuery<Response.RefreshTokenResponse>;
}
