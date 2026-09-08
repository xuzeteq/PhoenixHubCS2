using backend.Application.Dtos.User;

namespace backend.Application.Interfaces
{
    public interface IAdminUserService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    }
}
