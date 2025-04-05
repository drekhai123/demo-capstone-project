using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EduSource.Contract.Settings;
using System.Text;
using EduSource.Contract.Enumarations.Authentication;
using System.Security.Claims;

namespace EduSource.API.DepedencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR()
          .AddJsonProtocol(options =>
          {
              options.PayloadSerializerOptions.PropertyNamingPolicy = null;
          });

        var authenticationSetting = new AuthenticationSetting();
        configuration.GetSection(AuthenticationSetting.SectionName).Bind(authenticationSetting);


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer(options =>
       {
           options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = authenticationSetting.Issuer,
               ValidAudience = authenticationSetting.Audience,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSetting.AccessSecretToken)),
               ClockSkew = TimeSpan.Zero
           };

           options.Events = new JwtBearerEvents
           {
               OnAuthenticationFailed = context =>
               {
                   if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                   {
                       context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                   }
                   return Task.CompletedTask;
               },
           };
       });

        services.AddAuthorization(options =>
        {
            // Admin Policy
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireClaim(ClaimTypes.Role, ((int)RoleType.Admin).ToString()));

            // Staff Policy
            options.AddPolicy("StaffPolicy", policy =>
                policy.RequireClaim(ClaimTypes.Role, ((int)RoleType.Staff).ToString()));

            // Member Policy
            options.AddPolicy("MemberPolicy", policy =>
                policy.RequireClaim(ClaimTypes.Role, ((int)RoleType.Member).ToString()));

            // Member and Staff Policy
            options.AddPolicy("MemberStaffPolicy", policy =>
               policy.RequireAssertion(context =>
                   context.User.HasClaim(c => c.Type == ClaimTypes.Role &&
                       (c.Value == ((int)RoleType.Member).ToString() ||
                        c.Value == ((int)RoleType.Staff).ToString()))
               ));
        });

        return services;
    }
}
