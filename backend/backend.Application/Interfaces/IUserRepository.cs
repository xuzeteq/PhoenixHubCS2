using backend.Application.Dtos.User;
using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<User?> GetBySteamIdAsync(string steamId, CancellationToken cancellationToken = default);
        Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken = default);
        Task<List<User>> GetExpiredSubscribtions(CancellationToken cts = default);
        Task UpdateBalanceAsync(int id, decimal balance);
        Task PatchAsync(User user);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
