using backend.Application.Dtos.Auth;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Application.Options;
using backend.Application.Security;
using backend.Domain.Enums.User;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace backend.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IJwtTokenService _jwt;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository users, IJwtTokenService jwt, IOptions<JwtOptions> jwtOptions, ILogger<AuthService> logger)
        {
            _users = users;
            _jwt = jwt;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
        }

        public async Task<User> UpsertSteamUserAsync(string steamId, string username, string avatarUrl, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Получение пользователя по SteamID: {steamId}", steamId);
            var user = await _users.GetBySteamIdAsync(steamId, cancellationToken);

            if (user == null)
            {
                user = new User
                {
                    SteamId = steamId,
                    Username = string.IsNullOrWhiteSpace(username) ? "Player" : username,
                    AvatarUrl = avatarUrl,
                    CreatedAt = DateTime.UtcNow,
                    Role = RoleEnum.Пользователь,
                    LastLoginAt = DateTime.UtcNow,
                    Balance = 0
                };

                await _users.AddAsync(user, cancellationToken);
                _logger.LogInformation("Пользователь не найден, регистрация нового пользователя.");
            }
            else
            {
                user.LastLoginAt = DateTime.UtcNow;
                if (!string.IsNullOrWhiteSpace(username))
                    user.Username = username;
                if (!string.IsNullOrWhiteSpace(avatarUrl))
                    user.AvatarUrl = avatarUrl;
            }

            await _users.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<AuthTokensDto> IssueTokensAsync(User user, CancellationToken cancellationToken = default)
        {
            var accessToken = _jwt.CreateAccessToken(user, out var accessExpiresAt);
            var refreshToken = _jwt.CreateRefreshToken();
            var refreshExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays);

            user.RefreshToken = RefreshTokenHasher.Hash(refreshToken);
            user.RefreshTokenExpiry = refreshExpiresAt;
            await _users.SaveChangesAsync(cancellationToken);

            return new AuthTokensDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessExpiresAt,
                RefreshTokenExpiresAt = refreshExpiresAt
            };
        }

        public async Task<AuthTokensDto?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            var hash = RefreshTokenHasher.Hash(refreshToken);
            var user = await _users.GetByRefreshTokenHashAsync(hash, cancellationToken);

            if (user == null || user.RefreshTokenExpiry is null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiry = null;
                    await _users.SaveChangesAsync(cancellationToken);
                }

                return null;
            }

            return await IssueTokensAsync(user, cancellationToken);
        }

        public async Task<UserMeDto?> GetMeAsync(int userId, CancellationToken cancellationToken = default)
        {
            var user = await _users.GetByIdAsync(userId, cancellationToken);
            return user?.ToMeDto();
        }

        public async Task LogoutAsync(int? userId, string? refreshToken, CancellationToken cancellationToken = default)
        {
            User? user = null;

            if (userId is > 0)
                user = await _users.GetByIdAsync(userId.Value, cancellationToken);

            if (user == null && !string.IsNullOrWhiteSpace(refreshToken))
            {
                var hash = RefreshTokenHasher.Hash(refreshToken);
                user = await _users.GetByRefreshTokenHashAsync(hash, cancellationToken);
            }

            if (user == null)
                return;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _users.SaveChangesAsync(cancellationToken);
        }
    }
}
