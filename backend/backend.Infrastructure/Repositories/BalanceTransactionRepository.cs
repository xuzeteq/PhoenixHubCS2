using backend.Application.Interfaces;
using backend.Domain.Models;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Repositories
{
    public class BalanceTransactionRepository : IBalanceTransactionRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BalanceTransactionRepository> _logger;

        public BalanceTransactionRepository(AppDbContext context, ILogger<BalanceTransactionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(BalanceTransaction transaction)
        {
            await _context.BalanceTransactions.AddAsync(transaction);
        }

        public async Task<IReadOnlyList<BalanceTransaction>> GetAllAsync()
        {
            return await _context.BalanceTransactions
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<BalanceTransaction>> GetByUserIdAsync(int id)
        {
            var transactions = await _context.BalanceTransactions
                .AsNoTracking()
                .Where(t => t.UserId == id)
                .ToListAsync();

            return transactions;
        }
    }
}
