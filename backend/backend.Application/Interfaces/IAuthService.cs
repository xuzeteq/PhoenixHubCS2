using backend.Application.Dtos.Auth;
using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User> UpsertSteamUserAsync(string steamId, string username, string avatarUrl, CancellationToken cancellationToken = default);
        Task<AuthTokensDto> IssueTokensAsync(User user, CancellationToken cancellationToken = default);
        Task<AuthTokensDto?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<UserMeDto?> GetMeAsync(int userId, CancellationToken cancellationToken = default);
        Task LogoutAsync(int? userId, string? refreshToken, CancellationToken cancellationToken = default);
    }
}
