using backend.Application.Dtos.Logs;
using backend.Application.Mappings;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
    public class LogRepository
    {
        private readonly AppDbContext _context;

        public LogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyList<LogEntryDto> Logs, int TotalCount)> GetLogsAsync(LogQueryDto query)
        {
            var logsQuery = _context.Logs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(query.Level))
            {
                var levelInt = ParseLevel(query.Level);
                if (levelInt.HasValue)
                    logsQuery = logsQuery.Where(l => l.Level == levelInt.Value);
            }

            if (query.From.HasValue)
                logsQuery = logsQuery.Where(l => l.Timestamp >= query.From.Value);

            if (query.To.HasValue)
                logsQuery = logsQuery.Where(l => l.Timestamp <= query.To.Value);

            if (!string.IsNullOrEmpty(query.Search))
                logsQuery = logsQuery.Where(l => l.Message.Contains(query.Search));

            var totalCount = await logsQuery.CountAsync();

            var logs = await logsQuery
                .OrderByDescending(l => l.Timestamp)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var dtos = logs.Select(l => new LogEntryDto
            {
                Timestamp = l.Timestamp,
                Level = LogLevelMapping.ToLevelString(l.Level),
                Message = l.Message,
                Exception = l.Exception
            }).ToList();

            return (dtos, totalCount);
        }

        private static int? ParseLevel(string level) => level switch
        {
            "Verbose" => 0,
            "Debug" => 1,
            "Information" => 2,
            "Warning" => 3,
            "Error" => 4,
            "Fatal" => 5,
            _ => null
        };
    }
}