using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IAuditLogsService
    {
        Task LogAsync(AuditLog log, CancellationToken ct = default);
    }
}
