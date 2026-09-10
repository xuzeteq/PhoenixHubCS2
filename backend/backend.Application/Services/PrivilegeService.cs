using backend.Application.Dtos.Privilege;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class PrivilegeService : IPrivilegeService
    {
        private readonly IPrivilegeRepository _repo;
        private readonly ILogger<PrivilegeService> _logger;

        public PrivilegeService(IPrivilegeRepository repo, ILogger<PrivilegeService> logger)
        {
            _repo = repo;
            _logger = logger;
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
    }
}
