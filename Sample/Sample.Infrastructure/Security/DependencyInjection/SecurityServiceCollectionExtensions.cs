using System.IdentityModel.Tokens.Jwt;
using System.Text;
using EventManagement.Infrastructure.Jwt.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sample.Application.Core.Abstractions.Security;
using Sample.Infrastructure.Security.Jwt;
using Sample.Infrastructure.Security.Password;
using Sample.Infrastructure.Security.Policies;
using Sample.Infrastructure.Security.Policies.UserType;

namespace Sample.Infrastructure.Security.DependencyInjection;

public static class SecurityServiceCollectionExtensions
{
    internal static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(options => configuration.GetSection(JwtOptions.Jwt).Bind(options));
        var jwtOptions = configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>()!;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                };
            })
            .Services
            .AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            })
            .AddSingleton<IAuthorizationHandler, UserTypeHandler>()
            .AddSingleton<IAuthorizationPolicyProvider, SampleAuthorizationPolicyProvider>()
            .AddTransient<IJwtBearerGenerator, JwtBearerGenerator>()
            .AddTransient<IPasswordHasher,PasswordHasher>();
    }
}