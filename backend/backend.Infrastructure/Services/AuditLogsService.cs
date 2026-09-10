using backend.Application.Interfaces;
using backend.Domain.Models;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Services
{
    public class AuditLogsService : IAuditLogsService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuditLogsService> _logger;

        public AuditLogsService(AppDbContext context, ILogger<AuditLogsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogAsync(AuditLog log, CancellationToken ct = default)
        {
            log.Timestamp = DateTime.UtcNow;

            try
            {
                await _context.AuditLogs.AddAsync(log, ct);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Аудит лог не записан, ошибка.");
            }
        }
    }
}
