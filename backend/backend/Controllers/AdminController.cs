using backend.Application.Dtos.Logs;
using backend.Application.Dtos.User;
using backend.Application.Interfaces;
using backend.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminUserService _userService;
        private readonly LogRepository _logRepo;

        public AdminController(IAdminUserService userService, LogRepository logRepo)
        {
            _userService = userService;
            _logRepo = logRepo;
        }

        [HttpGet("users")]
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            return await _userService.GetAllUsersAsync();
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs([FromQuery] LogQueryDto query)
        {
            var (logs, totalLogs) = await _logRepo.GetLogsAsync(query);

            return Ok(new
            {
                logs,
                totalLogs,
                page = query.Page,
                pageSize = query.PageSize,
                totalPages = (int)Math.Ceiling(totalLogs / (double)query.PageSize)
            });
        }
    }
}
