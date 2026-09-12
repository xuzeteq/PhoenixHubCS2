using backend.Application.Dtos.Privilege;
using backend.Application.Results;

namespace backend.Application.Interfaces
{
    public interface IPrivilegeService
    {
        Task<IReadOnlyList<PrivilegeResponseDto>> GetAllPrivilegesAsync();
        Task<PrivilegeResponseDto> GetPrivilegeByIdAsync(int id);
        Task<PrivilegeResponseDto> CreatePrivilegeAsync(CreatePrivilegeDto dto);
        Task<PrivilegeResponseDto> UpdatePrivilegeAsync(int id, UpdatePrivilegeDto dto);
        Task<PrivilegePurchaseResult> PurchasePrivilegeAsync(int userId, int privilegeId, CancellationToken ct = default);
        Task DeletePrivilegeAsync(int id);
    }
}
