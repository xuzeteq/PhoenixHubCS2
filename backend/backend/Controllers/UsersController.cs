using backend.Application.Dtos.User;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            return await _service.GetAllUsersAsync();
        }

        [HttpGet("{steamId}")]
        public async Task<UserResponseDto> GetProfileBySteamId(string steamId, CancellationToken cts = default)
        {
            return await _service.GetProfileBySteamId(steamId, cts);
        }
    }
}
