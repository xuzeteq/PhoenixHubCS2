using backend.Application.Dtos.User;
using backend.Application.Interfaces;
using backend.Domain.Enums.User;
using backend.Domain.Models;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return _context.Users.ToListAsync(cancellationToken);
        }

        public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public Task<User?> GetBySteamIdAsync(string steamId, CancellationToken cancellationToken = default)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.SteamId == steamId, cancellationToken);
        }

        public async Task<List<User>> GetExpiredSubscribtions(CancellationToken cts = default)
        {
            return await _context.Users.Where(u => u.Role == RoleEnum.Phoenix && 
                u.SubscribtionExpireAt.HasValue && 
                u.SubscribtionExpireAt < DateTime.UtcNow)
            .ToListAsync(cts);
        }

        public Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken = default)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshTokenHash, cancellationToken);
        }

        public async Task UpdateBalanceAsync(int id, decimal balance)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new Exception("Пользователь не найден!");

            user.Balance += balance;
            await _context.SaveChangesAsync();
        }

        public async Task PatchAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
