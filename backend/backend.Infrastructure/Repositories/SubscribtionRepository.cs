using backend.Application.Interfaces;
using backend.Domain.Models;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class SubscribtionRepository : ISubscribtionRepository
    {
        private readonly AppDbContext _context;

        public SubscribtionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subscribtion>> GetAllAsync(CancellationToken cts = default)
        {
            return await _context.Subscribtions.ToListAsync(cts);
        }

        public async Task AddAsync(Subscribtion subscribtion, CancellationToken cts = default)
        {
            await _context.Subscribtions.AddAsync(subscribtion, cts);
        }

        public async Task UpdateAsync(Subscribtion subscribtion, CancellationToken cts = default)
        {
            _context.Subscribtions.Update(subscribtion);
            await _context.SaveChangesAsync();
        }
    }
}
