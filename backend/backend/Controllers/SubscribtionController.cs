using backend.Application.Dtos.Subscribtion;
using backend.Application.Interfaces;
using backend.Application.Results;
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

            var result = await _service.PurchaseSubscribeAsync(userId, cts);

            return result switch
            {
                SubscribtionPurchaseResult.Success => Ok(new { message = "Успешная покупка подписки." }),
                SubscribtionPurchaseResult.HaventMoney => BadRequest(new { message = "Недостаточно средств.", code = "HAVENT_MONEY" }),
                SubscribtionPurchaseResult.Error(var message) => StatusCode(500, message),
                _ => StatusCode(500, "Ошибка сервиса подписок.")
            };
        }
    }
}
