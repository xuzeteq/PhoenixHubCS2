using backend.Application.Dtos.Subscribtion;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribtionController : ControllerBase
    {
        private readonly ISubscribtionService _service;

        public SubscribtionController(ISubscribtionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<SubscribtionResponseDto>> GetAllSubscribtionsAsync(CancellationToken cts = default)
        {
            return await _service.GetAllSubscribtionsAsync(cts);
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseSubscribeAsync(CancellationToken cts = default)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                await _service.PurchaseSubscribeAsync(userId, cts);
                return Ok(new { message = "Подписка успешно оформлена" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
