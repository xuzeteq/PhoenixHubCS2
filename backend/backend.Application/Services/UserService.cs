using backend.Application.Dtos.User;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Domain.Exceptions;

namespace backend.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _repo.GetAllUsersAsync();
            return users.Select(u => u.ToDto()).ToList();
        }

        public async Task<UserResponseDto> GetProfileBySteamId(string steamId, CancellationToken cts = default)
        {
            var user = await _repo.GetBySteamIdAsync(steamId, cts);

            if (user == null)
                throw new NotFoundException("Пользователь", int.Parse(steamId));

            return user.ToDto();
        }
    }
}
