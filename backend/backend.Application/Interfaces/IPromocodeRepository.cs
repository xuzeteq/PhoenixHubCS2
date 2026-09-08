using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IPromocodeRepository
    {
        Task<List<Promocode>> GetAllAsync();
        Task<Promocode?> GetByIdAsync(int id);
        Task<Promocode?> GetByCodeAsync(string code);
        Task<bool> HasUserActivatedPromocode(int userId, int promocodeId);
        Task AddUsageAsync(PromocodeUsage usage);
        Task AddAsync(Promocode promocode);
        Task UpdateAsync(Promocode promocode);
        Task RemoveAsync(Promocode promocode);
    }
}
