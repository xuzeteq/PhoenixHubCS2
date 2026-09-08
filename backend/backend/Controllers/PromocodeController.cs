using backend.Application.Dtos.Promocode;
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
    public class PromocodeController : ControllerBase
    {
        private readonly IPromocodeService _service;

        public PromocodeController(IPromocodeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<PromocodeResponseDto>> GetAllPromocodesAsync()
        {
            return await _service.GetAllPromocodesAsync();
        }

        [HttpGet("{id}")]
        public async Task<PromocodeResponseDto> GetPromocodeByIdAsync(int id)
        {
            return await _service.GetPromocodeByIdAsync(id);
        }

        [HttpPost]
        public async Task<PromocodeResponseDto> CreatePromocodeAsync(CreatePromocodeDto dto)
        {
            return await _service.CreatePromocodeAsync(dto);
        }

        [HttpPost("activate")]
        [Authorize]
        public async Task<IActionResult> ActivatePromocode(ActivatePromocodeDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Неавторизованный пользователь.");
            }

            var result = await _service.ActivatePromocode(dto.Code, userId);

            return result switch
            {
                PromocodeActivationResult.Success(var promocode) => Ok(new
                {
                    giveBalance = promocode.GiveBalance,
                    code = promocode.Code,
                    message = "Промокод активирован!"
                }),

                PromocodeActivationResult.NotFound => NotFound(new { message = "Промокод не найден.", code = "NOT_FOUND" }),
                PromocodeActivationResult.Expired => BadRequest(new { message = "Промокод истек", code = "EXPIRED" }),
                PromocodeActivationResult.AlreadyUsed => BadRequest(new { message = "Вы уже активировали этот промокод", code = "ALREADY_USED" }),
                PromocodeActivationResult.UsageLimit => BadRequest(new { message = "Превышено кол-во активаций", code = "USAGE_LIMIT" }),
                PromocodeActivationResult.Error(var message) => StatusCode(500, new { message = "Ошибка", details = message }),
                _ => StatusCode(500)
            };
        }

        [HttpPatch("{id}")]
        public async Task<PromocodeResponseDto> UpdatePromocodeAsync(UpdatePromocodeDto dto, int id)
        {
            return await _service.UpdatePromocodeAsync(dto, id);
        }

        [HttpDelete("{id}")]
        public async Task<bool> RemovePromocodeAsync(int id)
        {
            return await _service.RemovePromocodeAsync(id);
        }
    }
}
