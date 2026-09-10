using backend.Application.Dtos.AuditLog;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class AuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyList<AuditLogResponseDto> Logs, int TotalCount)> GetLogsAsync(AuditLogQueryDto query)
        {
            var q = _context.AuditLogs.AsNoTracking().AsQueryable();

            if (query.UserId.HasValue)
                q = q.Where(l => l.UserId == query.UserId.Value);

            if (!string.IsNullOrEmpty(query.Action))
                q = q.Where(l => l.Action.Contains(query.Action));

            if (!string.IsNullOrEmpty(query.Username))
                q = q.Where(l => l.Username.Contains(query.Username));

            if (!string.IsNullOrEmpty(query.EntityName))
                q = q.Where(l => l.EntityName.Contains(query.EntityName));

            if (!string.IsNullOrEmpty(query.EntityType))
                q = q.Where(l => l.EntityType == query.EntityType);

            if (query.IsSuccess.HasValue)
                q = q.Where(l => l.IsSuccess == query.IsSuccess.Value);

            if (query.From.HasValue)
                q = q.Where(l => l.Timestamp >= query.From.Value);

            if (query.To.HasValue)
                q = q.Where(l => l.Timestamp <= query.To.Value);

            if (!string.IsNullOrEmpty(query.Search))
            {
                var search = query.Search.ToLower();
                q = q.Where(l =>
                    (l.EntityName != null && l.EntityName.ToLower().Contains(search)) ||
                    (l.Username != null && l.Username.ToLower().Contains(search)));
            }

            var totalCount = await q.CountAsync();

            var logs = await q
                .OrderByDescending(l => l.Timestamp)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(l => new AuditLogResponseDto
                {
                    Id = l.Id,
                    Timestamp = l.Timestamp,
                    UserId = l.UserId,
                    Username = l.Username,
                    Action = l.Action,
                    EntityType = l.EntityType,
                    EntityName = l.EntityName,
                    EntityId = l.EntityId,
                    OldValue = l.OldValue,
                    NewValue = l.NewValue,
                    Amount = l.Amount,
                    Currency = l.Currency,
                    DurationDays = l.DurationDays,
                    ValidUntil = l.ValidUntil,
                    StatusCode = l.StatusCode,
                    IsSuccess = l.IsSuccess,
                    ErrorMessage = l.ErrorMessage,
                    Metadata = l.Metadata
                })
                .ToListAsync();

            return (logs, totalCount);
        }
    }
}