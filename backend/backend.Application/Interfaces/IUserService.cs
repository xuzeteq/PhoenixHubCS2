using backend.Application.Dtos.User;

namespace backend.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetProfileBySteamId(string steamId, CancellationToken cts = default);
    }
}
