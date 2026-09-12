using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IBalanceTransactionService
    {
        Task<BalanceTransaction> CreateDepositAsync(int userId, decimal amount, string description, string? metadata);
        Task<BalanceTransaction> WithdrowDepositAsync(int userId, decimal amount, string description, string? metadata);
        Task<IReadOnlyList<BalanceTransaction>> GetUserTransactionsAsync(int id);
    }
}
