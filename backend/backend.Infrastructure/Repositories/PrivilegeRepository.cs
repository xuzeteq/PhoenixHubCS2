using backend.Application.Interfaces;
using backend.Domain.Models;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Repositories
{
    public class PrivilegeRepository : IPrivilegeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PrivilegeRepository> _logger;

        public PrivilegeRepository(AppDbContext context, ILogger<PrivilegeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Privilege privilege)
        {
            _logger.LogInformation("Добавление привилегии.");

            await _context.Privileges.AddAsync(privilege);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Привилегия {name} (ID: {id}) успешно создана.", privilege.Title, privilege.Id);
        }

        public async Task AddPrivilegeUserAsync(UserPrivilege userPrivilege)
        {
            await _context.UserPrivileges.AddAsync(userPrivilege);
        }

        public async Task<bool> HasUserPrivilegeAsync(int userId, int privilegeId)
        {
            return await _context.UserPrivileges.AnyAsync(u => u.UserId == userId && u.PrivilegeId == privilegeId);
        }

        public async Task AddFeaturesToPrivilegeAsync(int privilegeId, List<int> featureIds)
        {
            var existingFeatures = await _context.Features
                .Where(f => featureIds.Contains(f.Id))
                .Select(f => f.Id)
                .ToListAsync();

            var missingFeatures = featureIds.Except(existingFeatures).ToList();

            if (missingFeatures.Any())
            {
                _logger.LogWarning($"Возможности под ID: {string.Join(", ", missingFeatures)} не найдены.");
                throw new InvalidOperationException("Возможности не найдены.");
            }

            var links = featureIds.Select(f => new PrivilegeFeature
            {
                FeatureId = f,
                PrivilegeId = privilegeId
            });

            await _context.PrivilegeFeatures.AddRangeAsync(links);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Privilege privilege)
        {
            _logger.LogInformation("Привилегия {name} (ID: {id}) удалена.", privilege.Title, privilege.Id);

            _context.Privileges.Remove(privilege);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Privilege>> GetAllAsync()
        {
            return await _context.Privileges
                .Include(x => x.PrivilegeFeatures)
                    .ThenInclude(pf => pf.Feature)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Privilege?> GetByIdAsync(int id)
        {
            var privilege = await _context.Privileges
                .AsNoTracking()
                .Include(p => p.PrivilegeFeatures)
                    .ThenInclude(pf => pf.Feature)
                .FirstOrDefaultAsync(p => p.Id == id);

            return privilege;
        }

        public async Task<Privilege?> GetByTitleAsync(string title)
        {
            var privilege = await _context.Privileges.FirstOrDefaultAsync(p => p.Title == title);

            return privilege;
        }

        public async Task UpdateAsync(Privilege privilege)
        {
            _context.Privileges.Update(privilege);

            try
            {
                _logger.LogInformation("Обновление привилегии с ID: {id}", privilege.Id);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Ошибка обновления привилегии с ID: {id}.", privilege.Id);
                throw;
            }
        }
    }
}
