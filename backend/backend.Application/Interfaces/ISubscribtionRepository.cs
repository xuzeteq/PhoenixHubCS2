using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface ISubscribtionRepository
    {
        Task<List<Subscribtion>> GetAllAsync(CancellationToken cts = default);
        Task AddAsync(Subscribtion subscribtion, CancellationToken cts = default);
        Task UpdateAsync(Subscribtion subscribtion, CancellationToken cts = default);
    }
}
