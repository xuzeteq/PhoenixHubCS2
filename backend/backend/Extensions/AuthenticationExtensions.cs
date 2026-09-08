using AspNet.Security.OpenId.Steam;
using backend.API.Authentication;
using backend.Application.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace backend.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                      ?? throw new InvalidOperationException("Jwt configuration is missing");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey)),
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "PhoenixHub.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddSteam(options =>
            {
                options.ApplicationKey = configuration["Steam:ApiKey"];
                options.CallbackPath = "/signin-steam";
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Events.OnAuthenticated = context =>
                {
                    if (context.UserPayload is JsonDocument payload)
                    {
                        var players = payload.RootElement.GetProperty("response").GetProperty("players");
                        if (players.GetArrayLength() > 0)
                        {
                            var player = players[0];

                            if (player.TryGetProperty("avatarfull", out var avatar))
                                context.Identity!.AddClaim(new Claim("urn:steam:avatarfull", avatar.GetString() ?? string.Empty));

                            if (player.TryGetProperty("personaname", out var name))
                                context.Identity!.AddClaim(new Claim("urn:steam:personaname", name.GetString() ?? string.Empty));
                        }
                    }

                    return Task.CompletedTask;
                };

                options.Events.OnTicketReceived = SteamTicketReceivedHandler.HandleAsync;
            });

            return services;
        }
    }
}
