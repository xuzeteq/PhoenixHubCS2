using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IBalanceTransactionRepository
    {
        Task AddAsync(BalanceTransaction transaction);
        Task<IReadOnlyList<BalanceTransaction>> GetByUserIdAsync(int id);
        Task<IReadOnlyList<BalanceTransaction>> GetAllAsync();
    }
}
