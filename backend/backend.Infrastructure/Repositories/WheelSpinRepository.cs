using backend.Application.Interfaces;
using backend.Domain.Models;
using backend.Infrastructure.Data;
using backend.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Repositories
{
    public class WheelSpinRepository : IWheelSpinRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WheelSpinRepository> _logger;

        public WheelSpinRepository(AppDbContext context, ILogger<WheelSpinRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task RecordSpinWheelAsync(WheelItem spin)
        {
            await _context.AddAsync(spin);
        }

        public async Task<bool> HasSpunToday(int userId)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            return await _context.WheelItems
                .AnyAsync(ws => ws.UserId == userId && ws.CreatedAt >= today && ws.CreatedAt < tomorrow);
        }
    }
}
