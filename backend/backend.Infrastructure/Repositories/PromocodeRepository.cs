using backend.Application.Interfaces;
using backend.Domain.Models;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class PromocodeRepository : IPromocodeRepository
    {
        private readonly AppDbContext _context;

        public PromocodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Promocode>> GetAllAsync()
        {
            return await _context.Promocodes.AsNoTracking().ToListAsync();
        }

        public async Task<Promocode?> GetByIdAsync(int id)
        {
            var promocode = await _context.Promocodes.FirstOrDefaultAsync(p =>  p.Id == id);
            return promocode;
        }

        public async Task<Promocode?> GetByCodeAsync(string code)
        {
            var promocode = await _context.Promocodes.FirstOrDefaultAsync(p => p.Code == code);
            return promocode;
        }

        public async Task<bool> HasUserActivatedPromocode(int userId, int promocodeId)
        {
            return await _context.PromocodeUsages
                .AnyAsync(p => p.PromocodeId == promocodeId && p.UserId == userId);
        }

        public async Task AddUsageAsync(PromocodeUsage usage)
        {
            await _context.PromocodeUsages.AddAsync(usage);
        }

        public async Task AddAsync(Promocode promocode)
        {
            await _context.Promocodes.AddAsync(promocode);
        }

        public async Task UpdateAsync(Promocode promocode)
        {
            _context.Promocodes.Update(promocode);
        }

        public async Task RemoveAsync(Promocode promocode)
        {
            _context.Promocodes.Remove(promocode);
        }
    }
}
