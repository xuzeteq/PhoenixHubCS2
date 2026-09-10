using backend.Application.Dtos.Feature;

namespace backend.Application.Interfaces
{
    public interface IFeatureService
    {
        Task<IReadOnlyList<FeatureResponseDto>> GetAllFeaturesAsync();
        Task<FeatureResponseDto> GetFeatureByIdAsync(int id);
        Task<FeatureResponseDto> CreateFeatureAsync(CreateFeatureDto dto);
        Task<FeatureResponseDto> UpdateFeatureAsync(int id, UpdateFeatureDto dto);
        Task DeleteFeatureAsync(int id);
    }
}
