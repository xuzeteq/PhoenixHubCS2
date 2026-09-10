using backend.Application.Dtos.Feature;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository _repo;
        private readonly ILogger<FeatureService> _logger;

        public FeatureService(IFeatureRepository repo, ILogger<FeatureService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<FeatureResponseDto> CreateFeatureAsync(CreateFeatureDto dto)
        {
            var existingFeature = await _repo.GetByTitleAsync(dto.Title);

            if (existingFeature != null)
                throw new ConflictException("Возможность", dto.Title);

            var feature = new Feature
            {
                Title = dto.Title,
                Description = dto.Description,
                Icon = dto.Icon,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(feature);

            var created = await _repo.GetByIdAsync(feature.Id);

            return created!.ToDto();
        }

        public async Task DeleteFeatureAsync(int id)
        {
            var feature = await _repo.GetByIdAsync(id);

            if (feature == null)
                throw new NotFoundException("Возможность", id);

            await _repo.DeleteAsync(feature);
        }

        public async Task<IReadOnlyList<FeatureResponseDto>> GetAllFeaturesAsync()
        {
            var features = await _repo.GetAllAsync();

            return features.Select(f => f.ToDto()).ToList();
        }

        public async Task<FeatureResponseDto> GetFeatureByIdAsync(int id)
        {
            var feature = await _repo.GetByIdAsync(id);

            if (feature == null)
                throw new NotFoundException("Возможность", id);

            return feature.ToDto();
        }

        public async Task<FeatureResponseDto> UpdateFeatureAsync(int id, UpdateFeatureDto dto)
        {
            var feature = await _repo.GetByIdAsync(id);

            if (feature == null)
                throw new NotFoundException("Возможность", id);

            if (!string.IsNullOrEmpty(dto.Title))
                feature.Title = dto.Title;

            if (!string.IsNullOrEmpty(dto.Description))
                feature.Description = dto.Description;

            if (!string.IsNullOrEmpty(dto.Icon))
                feature.Icon = dto.Icon;

            await _repo.UpdateAsync(feature);

            return feature.ToDto();
        }
    }
}
