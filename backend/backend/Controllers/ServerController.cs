using backend.Application.Dtos.Server;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IServerService _service;

        public ServerController(IServerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<ServerResponseDto>> GetAllServersAsync()
        {
            return await _service.GetAllServersAsync();
        }

        [HttpGet("{id}")]
        public async Task<ServerResponseDto> GetServerByIdAsync(int id)
        {
            return await _service.GetServerByIdAsync(id);
        }

        [HttpPost]
        public async Task<ServerResponseDto> CreateServerAsync(CreateServerDto dto)
        {
            return await _service.CreateServerAsync(dto);
        }

        [HttpDelete("{id}")]
        public async Task<bool> RemoveServerAsync(int id)
        {
            return await _service.RemoveServerAsync(id);
        }
    }
}
