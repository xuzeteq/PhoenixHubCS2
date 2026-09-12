using backend.Application.Dtos.Subscribtion;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Domain.Constants;
using backend.Domain.Enums.User;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace backend.Application.Services
{
    public class SubscribtionService : ISubscribtionService
    {
        private readonly ISubscribtionRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _transaction;
        private readonly ILogger<SubscribtionService> _logger;
        private readonly IAuditLogsService _audit;
        private readonly IBalanceTransactionService _balanceTransaction;

        public SubscribtionService(ISubscribtionRepository repo, IUserRepository userRepo, ILogger<SubscribtionService> logger,
            IUnitOfWork transaction, IAuditLogsService audit, IBalanceTransactionService balanceTransaction)
        {
            _repo = repo;
            _logger = logger;
            _userRepo = userRepo;
            _transaction = transaction;
            _audit = audit;
            _balanceTransaction = balanceTransaction;
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
            {
                await _audit.LogAsync(new AuditLog
                {
                    EntityType = "Subscribtion",
                    EntityName = "Phoenix",
                    UserId = user.Id,
                    Username = user.Username,
                    Action = AuditActions.SUBSCRIBTION_FAILED_PURCHASE_HAVENT_BALANCE,
                    IsSuccess = false,
                    StatusCode = 400,
                });

                throw new BadRequestException("Недостаточно средств", "400");
            }

            await _transaction.BeginTransactionAsync(cts);

            var oldBalance = user.Balance;

            try
            {
                user.Role = RoleEnum.Phoenix;
                await _balanceTransaction.WithdrowDepositAsync(userId, subscribtionPrice, "Покупка подписки",
                    metadata: "Успешная активация подписки");

                bool isSubscribeActive = user.SubscribtionExpireAt.HasValue && user.SubscribtionExpireAt > DateTime.UtcNow;

                if (isSubscribeActive)
                {
                    user.SubscribtionExpireAt = user.SubscribtionExpireAt!.Value.AddDays(30);

                    await _audit.LogAsync(new AuditLog
                    {
                        EntityType = "Subscribtion",
                        EntityName = "Phoenix",
                        UserId = user.Id,
                        Username = user.Username,
                        Action = AuditActions.SUBSCRIBTION_RENEWED,
                        OldValue = oldBalance.ToString(),
                        NewValue = user.Balance.ToString(),
                        ValidUntil = user.SubscribtionExpireAt,
                        IsSuccess = true,
                        StatusCode = 200,
                    });
                }
                else
                {
                    user.SubscribtionExpireAt = DateTime.UtcNow.AddDays(30);

                    await _audit.LogAsync(new AuditLog
                    {
                        EntityType = "Subscribtion",
                        EntityName = "Phoenix",
                        UserId = user.Id,
                        Username = user.Username,
                        OldValue = oldBalance.ToString(),
                        NewValue = user.Balance.ToString(),
                        ValidUntil = user.SubscribtionExpireAt,
                        Action = AuditActions.SUBSCRIBTION_PURCHASED,
                        IsSuccess = true,
                        StatusCode = 200,
                    });
                }

                var subscribtion = new Subscribtion
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddDays(30)
                };

                await _repo.AddAsync(subscribtion, cts);

                await _transaction.SaveChangesAsync(cts);
                await _transaction.CommitTransactionAsync(cts);
            }
            catch (Exception ex)
            {
                await _audit.LogAsync(new AuditLog
                {
                    EntityType = "Subscribtion",
                    EntityName = "Phoenix",
                    UserId = user.Id,
                    Username = user.Username,
                    Action = AuditActions.SUBSCRIBTION_RENEWED,
                    IsSuccess = false,
                    StatusCode = 500,
                });

                _logger.LogError(ex, "Ошибка сервера подписок.");
                await _transaction.RollbackTransactionAsync(cts);
                throw;
            }
        }
    }
}
