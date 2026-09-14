using backend.Application.Dtos.Privilege;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Application.Results;
using backend.Domain.Constants;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace backend.Application.Services
{
    public class PrivilegeService : IPrivilegeService
    {
        private readonly IPrivilegeRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IBalanceTransactionService _balanceTransaction;
        private readonly IUnitOfWork _transaction;
        private readonly IAuditLogsService _audit;
        private readonly ILogger<PrivilegeService> _logger;

        public PrivilegeService(IPrivilegeRepository repo, ILogger<PrivilegeService> logger, IUserRepository userRepo,
            IBalanceTransactionService balanceTransaction, IUnitOfWork transaction, IAuditLogsService audit)
        {
            _repo = repo;
            _logger = logger;
            _userRepo = userRepo;
            _balanceTransaction = balanceTransaction;
            _transaction = transaction;
            _audit = audit;
        }

        public async Task<PrivilegeResponseDto> CreatePrivilegeAsync(CreatePrivilegeDto dto)
        {
            var existingPrivilege = await _repo.GetByTitleAsync(dto.Title);

            if (existingPrivilege != null)
                throw new ConflictException("Привилегия", existingPrivilege.Title);

            var privilege = new Privilege
            {
                Title = dto.Title,
                Price = dto.Price,
                OldPrice = dto.OldPrice,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(privilege);

            if (dto.FeatureIds.Any())
            {
                await _repo.AddFeaturesToPrivilegeAsync(privilege.Id, dto.FeatureIds);
            }

            var created = await _repo.GetByIdAsync(privilege.Id);

            return created!.ToDto();
        }

        public async Task DeletePrivilegeAsync(int id)
        {
            var privilege = await _repo.GetByIdAsync(id);

            if (privilege == null)
                throw new NotFoundException("Привилегия", id);

            await _repo.DeleteAsync(privilege);
        }

        public async Task<IReadOnlyList<PrivilegeResponseDto>> GetAllPrivilegesAsync()
        {
            var privileges = await _repo.GetAllAsync();

            return privileges.Select(p => p.ToDto()).ToList();
        }

        public async Task<PrivilegeResponseDto> GetPrivilegeByIdAsync(int id)
        {
            var privilege = await _repo.GetByIdAsync(id);

            if (privilege == null)
                throw new NotFoundException("Привилегия", id);

            return privilege.ToDto();
        }

        public async Task<PrivilegeResponseDto> UpdatePrivilegeAsync(int id, UpdatePrivilegeDto dto)
        {
            var privilege = await _repo.GetByIdAsync(id);

            if (privilege == null)
                throw new NotFoundException("Привилегия", id);

            if (!string.IsNullOrEmpty(dto.Title))
                privilege.Title = dto.Title;

            if (dto.Price.HasValue)
                privilege.Price = dto.Price.Value;

            if (dto.OldPrice.HasValue)
                privilege.OldPrice = dto.OldPrice.Value;

            await _repo.UpdateAsync(privilege);

            return privilege.ToDto();
        }

        public async Task<PrivilegePurchaseResult> PurchasePrivilegeAsync(int userId, int privilegeId, CancellationToken ct = default)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            var privilege = await _repo.GetByIdAsync(privilegeId);

            if (user == null)
                throw new UnauthorizedException("Пользователь неавторизован", "401");

            if (privilege == null)
                throw new NotFoundException("Privilege", privilegeId);

            if (await _repo.HasUserPrivilegeAsync(userId, privilegeId))
            {
                return new PrivilegePurchaseResult.AlreadyPurchased();
                throw new ConflictException("Privilege", privilege.Title);

            }

            if (user.Balance < privilege.Price)
            {
                await _audit.LogAsync(new AuditLog
                {
                    Action = AuditActions.PURCHASE_PRIVILEGE_FAILED_HAVENT_BALANCE,
                    EntityType = "Privilege",
                    EntityName = privilege.Title,
                    UserId = userId,
                    Username = user.Username,
                    IsSuccess = false,
                    StatusCode = 400,
                    ErrorMessage = "Недостаточно средств для покупки привилегии"
                });
                return new PrivilegePurchaseResult.HaventMoney();
            }

            var oldBalance = user.Balance;
            await _transaction.BeginTransactionAsync(ct);

            try
            {
                await _balanceTransaction.WithdrowDepositAsync(userId, privilege.Price, "Покупка привилегии",
                    metadata: JsonSerializer.Serialize(new
                    {
                        privilege = privilege.Title,
                        price = privilege.Price
                    }));

                var userPrivilege = new UserPrivilege
                {
                    PrivilegeId = privilege.Id,
                    UserId = user.Id,
                    PurchasedAt = DateTime.UtcNow,
                };

                await _repo.AddPrivilegeUserAsync(userPrivilege);

                await _audit.LogAsync(new AuditLog
                {
                    Action = AuditActions.PURCHASE_PRIVILEGE_SUCCESS,
                    OldValue = oldBalance.ToString(),
                    NewValue = user.Balance.ToString(),
                    EntityType = "Privilege",
                    EntityName = privilege.Title,
                    IsSuccess = true,
                    StatusCode = 200,
                    UserId = userId,
                    Username = user.Username
                });

                await _transaction.SaveChangesAsync(ct);
                await _transaction.CommitTransactionAsync(ct);

                return new PrivilegePurchaseResult.Success(privilege);
            }
            catch (Exception ex)
            {
                await _transaction.RollbackTransactionAsync(ct);
                _logger.LogError(ex, "Ошибка сервиса покупки привилегий!");
                return new PrivilegePurchaseResult.Error("Возникла ошибка сервиса.");
            }
        }
    }
}
