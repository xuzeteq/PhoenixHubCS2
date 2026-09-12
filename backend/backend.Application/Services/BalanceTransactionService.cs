using backend.Application.Interfaces;
using backend.Domain.Enums.Balance;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class BalanceTransactionService : IBalanceTransactionService
    {
        private readonly IBalanceTransactionRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<BalanceTransactionService> _logger;

        public BalanceTransactionService(IUserRepository userRepo, IBalanceTransactionRepository repo, ILogger<BalanceTransactionService> logger)
        {
            _repo = repo;
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<BalanceTransaction> CreateDepositAsync(int userId, decimal amount, string description, string? metadata)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User", userId);

            var oldBalance = user.Balance;

            user.Balance += amount;

            var transaction = new BalanceTransaction
            {
                UserId = user.Id,
                Type = BalanceTransactionTypeEnum.Пополнение,
                BalanceBefore = oldBalance,
                BalanceAfter = user.Balance,
                Description = description,
                ReferenceId = user.Id,
                Metadata = metadata,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(transaction);
            return transaction;
        }

        public async Task<IReadOnlyList<BalanceTransaction>> GetUserTransactionsAsync(int id)
        {
            return await _repo.GetByUserIdAsync(id);
        }

        public async Task<BalanceTransaction> WithdrowDepositAsync(int userId, decimal amount, string description, string? metadata)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User", userId);

            var oldBalance = user.Balance;

            user.Balance -= amount;

            var transaction = new BalanceTransaction
            {
                UserId = user.Id,
                Type = BalanceTransactionTypeEnum.Списание,
                BalanceBefore = oldBalance,
                BalanceAfter = user.Balance,
                Description = description,
                ReferenceId = user.Id,
                Metadata = metadata,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(transaction);
            return transaction;
        }
    }
}
