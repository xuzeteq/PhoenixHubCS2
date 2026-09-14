using backend.Application.Dtos.Privilege;
using backend.Application.Interfaces;
using backend.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivilegeController : ControllerBase
    {
        private readonly IPrivilegeService _service;

        public PrivilegeController(IPrivilegeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IReadOnlyList<PrivilegeResponseDto>> GetAllPrivilegesAsync()
        {
            return await _service.GetAllPrivilegesAsync();
        }

        [HttpGet("{id}")]
        public async Task<PrivilegeResponseDto> GetPrivilegeByIdAsync(int id)
        {
            return await _service.GetPrivilegeByIdAsync(id);
        }

        [HttpPost]
        public async Task<PrivilegeResponseDto> CreatePrivilegeAsync(CreatePrivilegeDto dto)
        {
            return await _service.CreatePrivilegeAsync(dto);
        }

        [HttpPost("purchase")]
        [Authorize]
        public async Task<IActionResult> PurchasePrivilegeAsync([FromBody] PurchasePrivilegeDto dto, CancellationToken ct = default)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
                return Unauthorized(new { message = "Неавторизованный пользователь." });

            var result = await _service.PurchasePrivilegeAsync(userId, dto.PrivilegeId, ct);

            return result switch
            {
                PrivilegePurchaseResult.Success(var privilege) => Ok(new
                {
                    message = "Привелегия успешно приоберетена!"
                }),

                PrivilegePurchaseResult.HaventMoney => BadRequest(new { message = "Недостаточно средств!", code = "HAVENT_MONEY" }),
                PrivilegePurchaseResult.AlreadyPurchased => BadRequest(new { message = "Привелегия уже приобретена", code = "ALREADY_PURCHASED" }),
                PrivilegePurchaseResult.Error(var message) => StatusCode(500, new { message = "Ошибка", details = message }),
                _ => StatusCode(500)
            };


        }

        [HttpPatch("{id}")]
        public async Task<PrivilegeResponseDto> UpdatePrivilegeAsync(int id, UpdatePrivilegeDto dto)
        {
            return await _service.UpdatePrivilegeAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrivilegeAsync(int id)
        {
            await _service.DeletePrivilegeAsync(id);
            return NoContent();
        }
    }
}
