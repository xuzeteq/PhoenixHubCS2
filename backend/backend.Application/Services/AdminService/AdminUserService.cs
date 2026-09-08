using backend.Application.Dtos.User;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Domain.Models;

namespace backend.Application.Services.AdminService
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUserRepository _repo;

        public AdminUserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            var users = await _repo.GetAllUsersAsync(cancellationToken);

            return users.Select(u => u.ToDto()).ToList();
        }
    }
}
