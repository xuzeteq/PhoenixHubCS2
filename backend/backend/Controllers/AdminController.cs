using backend.Application.Dtos.AuditLog;
using backend.Application.Dtos.Logs;
using backend.Application.Dtos.User;
using backend.Application.Interfaces;
using backend.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Serilog;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminUserService _userService;
        private readonly LogRepository _logRepo;
        private readonly AuditLogRepository _auditRepo;

        public AdminController(IAdminUserService userService, LogRepository logRepo, AuditLogRepository auditRepo)
        {
            _userService = userService;
            _logRepo = logRepo;
            _auditRepo = auditRepo;
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

        [HttpGet("audit")]
        public async Task<IActionResult> GetLogs([FromQuery] AuditLogQueryDto query)
        {
            var (audit, logCount) = await _auditRepo.GetLogsAsync(query);

            return Ok(new
            {
                audit,
                totalLogs = logCount,
                page = query.Page,
                pageSize = query.PageSize,
                totalPages = (int)Math.Ceiling(logCount / (double)query.PageSize)
            });
        }

        [HttpGet("audit/user/{id}")]
        public async Task<IActionResult> GetUserLogs(int userId, [FromQuery] AuditLogQueryDto query)
        {
            query.UserId = userId;
            var (audit, logCount) = await _auditRepo.GetLogsAsync(query);

            return Ok(new
            {
                audit,
                totalLogs = logCount,
                page = query.Page,
                pageSize = query.PageSize,
                totalPages = (int)Math.Ceiling(logCount / (double)query.PageSize)
            });
        }
    }
}
