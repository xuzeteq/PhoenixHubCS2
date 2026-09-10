using backend.Application.Dtos.Subscribtion;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Domain.Enums.User;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class SubscribtionService : ISubscribtionService
    {
        private readonly ISubscribtionRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _transaction;
        private readonly ILogger<SubscribtionService> _logger;

        public SubscribtionService(ISubscribtionRepository repo, IUserRepository userRepo, ILogger<SubscribtionService> logger,
            IUnitOfWork transaction)
        {
            _repo = repo;
            _logger = logger;
            _userRepo = userRepo;
            _transaction = transaction;
        }

        public async Task<List<SubscribtionResponseDto>> GetAllSubscribtionsAsync(CancellationToken cts = default)
        {
            var subscribtions = await _repo.GetAllAsync(cts);
            return subscribtions.Select(s => s.ToDto()).ToList();
        }

        public async Task PurchaseSubscribeAsync(int userId, CancellationToken cts = default)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("Пользователь", userId);

            decimal subscribtionPrice = 249m;

            if (user.Balance < subscribtionPrice)
                throw new Exception("Недостаточно средств для покупки подписки.");

            await _transaction.BeginTransactionAsync(cts);

            try
            {
                user.Balance -= subscribtionPrice;
                user.Role = RoleEnum.Phoenix;

                bool isSubscribeActive = user.SubscribtionExpireAt.HasValue && user.SubscribtionExpireAt > DateTime.UtcNow;

                if (isSubscribeActive)
                {
                    user.SubscribtionExpireAt = user.SubscribtionExpireAt!.Value.AddDays(30);
                    _logger.LogInformation("Пользователь {username} продлил подписку Phoenix на 1 мес.", user.Username);
                }
                else
                {
                    user.SubscribtionExpireAt = DateTime.UtcNow.AddDays(30);
                    _logger.LogInformation("Пользователь {username} приобрел подписку Phoenix на 1 мес.", user.Username);
                }

                var subscribtion = new Subscribtion
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddDays(30)
                };

                await _repo.AddAsync(subscribtion, cts);

                await _userRepo.PatchAsync(user);

                await _transaction.SaveChangesAsync(cts);
                await _transaction.CommitTransactionAsync(cts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка подписки!");
                await _transaction.RollbackTransactionAsync(cts);
                throw;
            }
        }
    }
}
