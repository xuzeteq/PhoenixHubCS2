using backend.Application.Dtos.Privilege;

namespace backend.Application.Interfaces
{
    public interface IPrivilegeService
    {
        Task<IReadOnlyList<PrivilegeResponseDto>> GetAllPrivilegesAsync();
        Task<PrivilegeResponseDto> GetPrivilegeByIdAsync(int id);
        Task<PrivilegeResponseDto> CreatePrivilegeAsync(CreatePrivilegeDto dto);
        Task<PrivilegeResponseDto> UpdatePrivilegeAsync(int id, UpdatePrivilegeDto dto);
        Task DeletePrivilegeAsync(int id);
    }
}
