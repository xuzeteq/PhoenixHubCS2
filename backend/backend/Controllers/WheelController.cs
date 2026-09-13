using backend.Application.Interfaces;
using backend.Application.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WheelController : ControllerBase
    {
        private readonly IWheelSpinService _service;

        public WheelController(IWheelSpinService service)
        {
            _service = service;
        }

        [HttpPost("spin")]
        public async Task<IActionResult> SpinAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Неавторизованный пользователь.");
            }

            var result = await _service.SpinAsync(userId);

            return result switch
            {
                WheelSpinResult.Success(var item) => Ok(new { message = "Успешно открутили.", amount = item.Amount }),
                WheelSpinResult.AlreadySpun => BadRequest(new { message = "Вы уже крутили колесо сегодня!" }),
                WheelSpinResult.Error => StatusCode(500),
                _ => StatusCode(500, new { message = "Ошибка!" })
            };
        }
    }
}
