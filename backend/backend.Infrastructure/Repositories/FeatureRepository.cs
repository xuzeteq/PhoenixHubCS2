using backend.Application.Interfaces;
using backend.Domain.Models;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Repositories
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FeatureRepository> _logger;

        public FeatureRepository(AppDbContext context, ILogger<FeatureRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Feature feature)
        {
            _logger.LogInformation("Добавление возможности.");

            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Возможность: {name} (ID: {id}) успешно создана.", feature.Title, feature.Id);
        }

        public async Task DeleteAsync(Feature feature)
        {
            _logger.LogInformation("Возможность: {name} (ID: {id}) удалена.", feature.Title, feature.Id);

            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Feature>> GetAllAsync()
        {
            return await _context.Features
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Feature?> GetByTitleAsync(string title)
        {
            var feature = await _context.Features
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Title == title);

            return feature;
        }

        public async Task<Feature?> GetByIdAsync(int id)
        {
            var feature = await _context.Features
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return feature;
        }

        public async Task UpdateAsync(Feature feature)
        {
            _context.Features.Update(feature);

            try
            {
                _logger.LogInformation("Обновление возможности с ID: {id}", feature.Id);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Ошибка обновления возможности с ID: {id}.", feature.Id);
                throw;
            }
        }
    }
}
