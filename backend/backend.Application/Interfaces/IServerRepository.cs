using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IServerRepository
    {
        Task<List<Server>> GetAllAsync();
        Task<Server?> GetByIdAsync(int id);
        Task AddAsync(Server server);
        Task RemoveAsync(Server server);
        Task PatchAsync(Server server);
        Task UpdateBatchAsync(List<Server> servers, CancellationToken cts = default);
    }
}
