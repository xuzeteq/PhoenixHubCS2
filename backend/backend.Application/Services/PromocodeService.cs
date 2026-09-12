using backend.Application.Dtos.Promocode;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Application.Results;
using backend.Domain.Constants;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace backend.Application.Services
{
    public class PromocodeService : IPromocodeService
    {
        private readonly IPromocodeRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<PromocodeService> _logger;
        private readonly IAuditLogsService _audit;
        private readonly IBalanceTransactionService _transactionService;
        private readonly IUnitOfWork _transaction;

        public PromocodeService(ILogger<PromocodeService> logger, IPromocodeRepository repo, IUserRepository userRepo,
            IAuditLogsService audit, IUnitOfWork transction, IBalanceTransactionService transactionService)
        {
            _logger = logger;
            _repo = repo;
            _userRepo = userRepo;
            _audit = audit;
            _transaction = transction;
            _transactionService = transactionService;
        }

        public async Task<PromocodeResponseDto> CreatePromocodeAsync(CreatePromocodeDto dto)
        {
            var existedPromocode = await _repo.GetByCodeAsync(dto.Code);

            if (existedPromocode != null)
                throw new ConflictException("Промокод", existedPromocode.Code);

            var promocode = new Promocode
            {
                Code = dto.Code,
                GiveBalance = dto.GiveBalance,
                MaxUses = dto.MaxUses,
                UsedCount = 0,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddDays(dto.DaysActive)
            };

            await _repo.AddAsync(promocode);

            return promocode.ToDto();
        }

        public async Task<List<PromocodeResponseDto>> GetAllPromocodesAsync()
        {
            var promocodes = await _repo.GetAllAsync();
            return promocodes.Select(p => p.ToDto()).ToList();
        }

        public async Task<PromocodeResponseDto> GetPromocodeByIdAsync(int id)
        {
            var promocode = await _repo.GetByIdAsync(id);

            if (promocode == null)
                throw new NotFoundException("Промокод", id);

            return promocode.ToDto();
        }

        public async Task<PromocodeActivationResult> ActivatePromocode(string code, int userId, CancellationToken ct = default)
        {
            var promocode = await _repo.GetByCodeAsync(code);
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                throw new UnauthorizedException("Не авторизован", "401");

            if (promocode == null)
            {
                await _audit.LogAsync(new AuditLog
                {
                    EntityType = "Promocode",
                    EntityName = code,
                    UserId = userId,
                    Username = user.Username,
                    Action = AuditActions.PROMOCODE_NOT_FOUND,
                    IsSuccess = false,
                    StatusCode = 404,
                    ErrorMessage = "Промокод не найден."
                });

                return new PromocodeActivationResult.NotFound();
            }

            if (promocode.UsedCount >= promocode.MaxUses)
            {
                await _audit.LogAsync(new AuditLog
                {
                    EntityType = "Promocode",
                    EntityName = promocode.Code,
                    UserId = userId,
                    Username = user.Username,
                    Action = AuditActions.PROMOCODE_USAGE_LIMIT,
                    IsSuccess = false,
                    StatusCode = 400,
                    ErrorMessage = "Промокод уже использован максимальное кол-во раз."
                });

                return new PromocodeActivationResult.UsageLimit();
            }

            if (promocode.ExpireAt <= DateTime.UtcNow)
            {
                await _audit.LogAsync(new AuditLog
                {
                    EntityType = "Promocode",
                    EntityName = promocode.Code,
                    UserId = userId,
                    Username = user.Username,
                    Action = AuditActions.PROMOCODE_EXPIRED,
                    IsSuccess = false,
                    StatusCode = 400,
                    ErrorMessage = "Срок действия промокода истек."
                });

                return new PromocodeActivationResult.Expired();
            }

            if (await _repo.HasUserActivatedPromocode(userId, promocode.Id))
            {
                await _audit.LogAsync(new AuditLog
                {
                    EntityType = "Promocode",
                    EntityName = promocode.Code,
                    UserId = userId,
                    Username = user.Username,
                    Action = AuditActions.PROMOCODE_ALREADY_USED,
                    IsSuccess = false,
                    StatusCode = 400,
                    ErrorMessage = "Пользователь уже активировал этот промокод."
                });

                return new PromocodeActivationResult.AlreadyUsed();
            }

            await _transaction.BeginTransactionAsync(ct);

            try
            {
                promocode.UsedCount++;

                var usage = new PromocodeUsage
                {
                    PromocodeId = promocode.Id,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                };
                await _repo.AddUsageAsync(usage);

                await _transactionService.CreateDepositAsync(userId, promocode.GiveBalance,
                    $"Активация промокода {promocode.Code}", metadata: JsonSerializer.Serialize(new
                    {
                        id = promocode.Id,
                        code = promocode.Code,
                        message = "Успешная активация промокода"
                    }));

                await _transaction.SaveChangesAsync(ct);
                await _transaction.CommitTransactionAsync(ct);
                
                await _audit.LogAsync(new AuditLog
                {
                    EntityType = "Promocode",
                    EntityName = promocode.Code,
                    UserId = userId,
                    Username = user.Username,
                    Action = AuditActions.PROMOCODE_ACTIVATED,
                    IsSuccess = true,
                    StatusCode = 200,
                });
                
                return new PromocodeActivationResult.Success(promocode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Промокод {code} не активирован.", code);

                await _transaction.RollbackTransactionAsync(ct);

                await _audit.LogAsync(new AuditLog
                {
                    EntityType = "Promocode",
                    EntityName = promocode.Code,
                    UserId = userId,
                    Username = user.Username,
                    Action = AuditActions.PROMOCODE_FAILED,
                    IsSuccess = true,
                    StatusCode = 200,
                });

                return new PromocodeActivationResult.Error(ex.Message);
            }
        }

        public async Task<bool> RemovePromocodeAsync(int id)
        {
            var promocode = await _repo.GetByIdAsync(id);

            if (promocode == null)
                throw new NotFoundException("Промокод", id);

            await _repo.RemoveAsync(promocode);
            return true;
        }

        public async Task<PromocodeResponseDto> UpdatePromocodeAsync(UpdatePromocodeDto dto, int id)
        {
            var promocode = await _repo.GetByIdAsync(id);

            if (promocode == null)
                throw new NotFoundException("Промокод", id);
            
            await _repo.UpdateAsync(promocode);
            return promocode.ToDto();
        }
    }
}
