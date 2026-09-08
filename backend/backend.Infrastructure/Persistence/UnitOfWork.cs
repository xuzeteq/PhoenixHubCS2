using backend.Application.Interfaces;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace backend.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync(CancellationToken cts = default)
            => _transaction = await _context.Database.BeginTransactionAsync(cts);

        public async Task CommitTransactionAsync(CancellationToken cts = default)
        {
            if (_transaction == null) throw new Exception("Транзакция не найдена.");

            await _transaction.CommitAsync(cts);
            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken cts = default)
        {
            if (_transaction == null) throw new Exception("Транзакция не найдена.");

            await _transaction.RollbackAsync(cts);
            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cts = default)
            => await _context.SaveChangesAsync(cts);
    }
}
