using backend.Application.Interfaces;
using backend.Domain.Enums.User;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class SubscribtionsCleanupService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _transaction;
        private readonly ILogger<SubscribtionsCleanupService> _logger;

        public SubscribtionsCleanupService(IUserRepository userRepo,
            IUnitOfWork transaction,
            ILogger<SubscribtionsCleanupService> logger)
        {
            _logger = logger;
            _transaction = transaction;
            _userRepo = userRepo;
        }

        public async Task CleanupSubscribtions(CancellationToken cts = default)
        {
            var expiredUsers = await _userRepo.GetExpiredSubscribtions(cts);

            if (expiredUsers == null)
            {
                _logger.LogInformation("Нет просроченных подписок.");
                return;
            }

            _logger.LogInformation("Найдено {count} просроченных подписок.", expiredUsers.Count());

            await _transaction.BeginTransactionAsync(cts);

            try
            {
                foreach (var user in expiredUsers)
                {
                    user.Role = RoleEnum.Пользователь;
                    user.SubscribtionExpireAt = null;
                    await _userRepo.PatchAsync(user);
                }

                await _transaction.SaveChangesAsync(cts);
                await _transaction.CommitTransactionAsync(cts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при очистке просроченных подписок.");
                await _transaction.RollbackTransactionAsync(cts);
                throw;
            }
        }
    }
}
