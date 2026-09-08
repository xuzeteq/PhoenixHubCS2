using backend.Application.Interfaces;
using backend.Application.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace backend.API.Authentication
{
    public static class SteamTicketReceivedHandler
    {
        public static async Task HandleAsync(TicketReceivedContext context)
        {
            var rawSteamId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var frontendUrl = context.HttpContext.RequestServices
                .GetRequiredService<IOptions<FrontendOptions>>().Value.Url.TrimEnd('/');

            if (string.IsNullOrEmpty(rawSteamId))
            {
                context.Response.Redirect($"{frontendUrl}/login?error=no_steam_id");
                context.HandleResponse();
                return;
            }

            var steamId = rawSteamId.Contains('/') ? rawSteamId.Split('/').Last() : rawSteamId;
            var username = context.Principal?.FindFirstValue("urn:steam:personaname")
                           ?? context.Principal?.FindFirstValue(ClaimTypes.Name)
                           ?? "Player";
            var avatarUrl = context.Principal?.FindFirstValue("urn:steam:avatarfull") ?? string.Empty;

            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
            var user = await authService.UpsertSteamUserAsync(steamId, username, avatarUrl, context.HttpContext.RequestAborted);
            var tokens = await authService.IssueTokensAsync(user, context.HttpContext.RequestAborted);

            var redirectUrl =
                $"{frontendUrl}/auth/callback" +
                $"?token={Uri.EscapeDataString(tokens.AccessToken)}" +
                $"&refreshToken={Uri.EscapeDataString(tokens.RefreshToken)}";

            context.Response.Redirect(redirectUrl);
            context.HandleResponse();
        }
    }
}
