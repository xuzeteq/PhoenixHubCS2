using backend.Application.Dtos.Promocode;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Application.Results;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class PromocodeService : IPromocodeService
    {
        private readonly IPromocodeRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<PromocodeService> _logger;

        public PromocodeService(ILogger<PromocodeService> logger, IPromocodeRepository repo, IUserRepository userRepo)
        {
            _logger = logger;
            _repo = repo;
            _userRepo = userRepo;
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

        public async Task<PromocodeActivationResult> ActivatePromocode(string code, int userId)
        {
            var promocode = await _repo.GetByCodeAsync(code);

            if (promocode == null)
            {
                _logger.LogWarning("Промокод {code} не найден.", code);
                return new PromocodeActivationResult.NotFound();
            }

            if (promocode.UsedCount >= promocode.MaxUses)
            {
                _logger.LogWarning("Промокод {code} уже использован максимальное кол-во раз.", code);
                return new PromocodeActivationResult.UsageLimit();
            }

            if (promocode.ExpireAt <= DateTime.UtcNow)
            {
                _logger.LogWarning("Промокод {code} неактивен.", code);
                return new PromocodeActivationResult.Expired();
            }

            if (await _repo.HasUserActivatedPromocode(userId, promocode.Id))
            {
                _logger.LogWarning("Пользователь {userId} уже использовал промокод {code}", userId, code);
                return new PromocodeActivationResult.AlreadyUsed();
            }

            try
            {
                promocode.UsedCount++;

                await _repo.UpdateAsync(promocode);

                var usage = new PromocodeUsage
                {
                    PromocodeId = promocode.Id,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                };
                await _repo.AddUsageAsync(usage);
                await _userRepo.UpdateBalanceAsync(userId, promocode.GiveBalance);

                _logger.LogInformation("Пользователь {userId} активировал проомокод {code}", userId, code);

                return new PromocodeActivationResult.Success(promocode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Промокод {code} не активирован.", code);
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
