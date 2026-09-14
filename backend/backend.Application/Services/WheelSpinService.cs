using backend.Application.Dtos.WheelSpin;
using backend.Application.Interfaces;
using backend.Application.Results;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class WheelSpinService : IWheelSpinService
    {
        private readonly IWheelSpinRepository _repo;
        private readonly ILogger<WheelSpinService> _logger;
        private readonly IUnitOfWork _transaction;
        private readonly IBalanceTransactionService _balanceTransaction;
        private readonly IAuditLogsService _audit;
        private readonly IUserRepository _userRepo;
        private readonly Random _random = new();

        public WheelSpinService(IWheelSpinRepository repo, ILogger<WheelSpinService> logger, IUserRepository userRepo,
            IUnitOfWork transaction, IBalanceTransactionService balanceTransaction, IAuditLogsService audit)
        {
            _audit = audit;
            _userRepo = userRepo;
            _balanceTransaction = balanceTransaction;
            _logger = logger;
            _repo = repo;
            _transaction = transaction;
        }

        public async Task<WheelSpinResult> SpinAsync(int userId)
        {

            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null) throw new UnauthorizedException("Неавторизован.", "401");

            if (await _repo.HasSpunToday(userId))
            {
                return new WheelSpinResult.AlreadySpun();
            }

            var prizes = new List<(int weight, string rarity, decimal amount)>
            {
                ( 30, "Обычный", 5 ),
                ( 20, "Обычный", 10 ),
                ( 10, "Обычный", 15 ),
                ( 8, "Необычный", 25 ),
                ( 8, "Необычный", 30 ),
                ( 8, "Необычный", 35 ),
                ( 5, "Редкий", 50 ),
                ( 5, "Редкий", 60 ),
                ( 5, "Эпический", 75 ),
                ( 1, "Легендарный", 125 ),
            };

            var ( weight, rarity, amount ) = GetWeightedRandom(prizes);

            await _transaction.BeginTransactionAsync();

            try
            {
                await _balanceTransaction.CreateDepositAsync(userId, amount, "Прокрутка колеса фортуны", metadata: null);

                var spin = new WheelItem
                {
                    UserId = userId,
                    Amount = amount,
                    Rarity = rarity.ToString(),
                    Weight = weight,
                    CreatedAt = DateTime.UtcNow,
                };

                await _repo.RecordSpinWheelAsync(spin);

                await _audit.LogAsync(new AuditLog
                {
                    Action = $"открутил колесо фортуны. Приз: {amount} рублей.",
                    Amount = amount,
                    EntityType = "Wheel Of Fortune",
                    IsSuccess = true,
                    StatusCode = 200,
                    UserId = userId,
                    Username = user.Username
                });

                await _transaction.SaveChangesAsync();
                await _transaction.CommitTransactionAsync();

                return new WheelSpinResult.Success(spin);
            }
            catch (Exception ex)
            {
                await _transaction.RollbackTransactionAsync();
                return new WheelSpinResult.Error(ex.Message);
            }
        }

        private (int Weight, string Rarity, decimal Amount) GetWeightedRandom(List<(int Weight, string Rarity, decimal Amount)> prizes)
        {
            int totalWeight = prizes.Sum(p => p.Weight);
            int randomNumber = _random.Next(0, totalWeight);

            int currentWeight = 0;
            foreach (var prize in prizes)
            {
                currentWeight += prize.Weight;
                if (randomNumber < currentWeight)
                    return prize;
            }

            return prizes.Last();
        }
    }
}
