using backend.Application.Dtos.Promocode;
using backend.Application.Results;

namespace backend.Application.Interfaces
{
    public interface IPromocodeService
    {
        Task<List<PromocodeResponseDto>> GetAllPromocodesAsync();
        Task<PromocodeResponseDto> GetPromocodeByIdAsync(int id);
        Task<PromocodeResponseDto> CreatePromocodeAsync(CreatePromocodeDto dto);
        Task<PromocodeResponseDto> UpdatePromocodeAsync(UpdatePromocodeDto dto, int id);
        Task<PromocodeActivationResult> ActivatePromocode(string code, int userId);
        Task<bool> RemovePromocodeAsync(int id);
    }
}
