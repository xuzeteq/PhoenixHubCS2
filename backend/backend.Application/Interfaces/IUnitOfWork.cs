namespace backend.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cts = default);
        Task BeginTransactionAsync(CancellationToken cts = default);
        Task CommitTransactionAsync(CancellationToken cts = default);
        Task RollbackTransactionAsync(CancellationToken cts = default);
    }
}
