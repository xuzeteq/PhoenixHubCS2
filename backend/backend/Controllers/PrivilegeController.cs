using backend.Application.Dtos.Privilege;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
