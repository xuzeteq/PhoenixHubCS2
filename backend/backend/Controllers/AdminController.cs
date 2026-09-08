using backend.Application.Dtos.User;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminUserService _userService;

        public AdminController(IAdminUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            return await _userService.GetAllUsersAsync();
        }
    }
}
