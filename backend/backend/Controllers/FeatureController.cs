using backend.Application.Dtos.Feature;
using backend.Application.Dtos.Privilege;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureService _service;

        public FeatureController(IFeatureService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IReadOnlyList<FeatureResponseDto>> GetAllFeaturesAsync()
        {
            return await _service.GetAllFeaturesAsync();
        }

        [HttpGet("{id}")]
        public async Task<FeatureResponseDto> GetFeatureByIdAsync(int id)
        {
            return await _service.GetFeatureByIdAsync(id);
        }

        [HttpPost]
        public async Task<FeatureResponseDto> CreateFeatureAsync(CreateFeatureDto dto)
        {
            return await _service.CreateFeatureAsync(dto);
        }

        [HttpPatch("{id}")]
        public async Task<FeatureResponseDto> UpdateFeatureAsync(int id, UpdateFeatureDto dto)
        {
            return await _service.UpdateFeatureAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeatureAsync(int id)
        {
            await _service.DeleteFeatureAsync(id);
            return NoContent();
        }
    }
}
