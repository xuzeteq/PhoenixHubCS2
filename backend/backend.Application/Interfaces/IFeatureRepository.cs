using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IFeatureRepository
    {
        Task<IReadOnlyList<Feature>> GetAllAsync();
        Task<Feature?> GetByIdAsync(int id);
        Task<Feature?> GetByTitleAsync(string title);
        Task AddAsync(Feature feature);
        Task UpdateAsync(Feature feature);
        Task DeleteAsync(Feature feature);
    }
}
