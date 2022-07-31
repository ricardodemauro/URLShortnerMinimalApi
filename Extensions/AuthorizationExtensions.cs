using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using URLShortnerMinimalApi.Authorizations;

namespace URLShortnerMinimalApi.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAndConfigureAuth0(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(opts =>
            {
                var scopeReq = new HasScopeRequirement("create:url", configuration["Oidc:Issuer"]);
                opts.AddPolicy("create:url", policy => policy.Requirements.Add(scopeReq));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = configuration["Oidc:Authority"];
                options.Audience = configuration["Oidc:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                };
            });

            return services;
        }
    }
}
