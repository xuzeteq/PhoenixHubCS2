using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IPrivilegeRepository
    {
        Task<IReadOnlyList<Privilege>> GetAllAsync();
        Task<Privilege?> GetByIdAsync(int id);
        Task<Privilege?> GetByTitleAsync(string title);
        Task AddAsync(Privilege privilege);
        Task AddFeaturesToPrivilegeAsync(int privilegeId, List<int> featureIds);
        Task UpdateAsync(Privilege privilege);
        Task DeleteAsync(Privilege privilege);
        Task<bool> HasUserPrivilegeAsync(int userId, int privilegeId);
        Task AddPrivilegeUserAsync(UserPrivilege userPrivilege);
    }
}
