using backend.Application.Interfaces;
using backend.Domain.Models;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ServerRepository> _logger;

        public ServerRepository(AppDbContext context, ILogger<ServerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Server>> GetAllAsync()
        {
            return await _context.Servers.AsNoTracking().ToListAsync();
        }

        public async Task<Server?> GetByIdAsync(int id)
        {
            return await _context.Servers.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Server server)
        {
            await _context.Servers.AddAsync(server);
            await _context.SaveChangesAsync();
        }

        public async Task PatchAsync(Server server)
        {
            _context.Servers.Update(server);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Server server)
        {
            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(List<Server> servers, CancellationToken cts = default)
        {
            foreach (var server in servers)
            {
                _context.Servers.Update(server);
            }
            await _context.SaveChangesAsync(cts);
        }
    }
}
