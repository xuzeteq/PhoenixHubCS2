using backend.Application.Dtos.Server;
using backend.Application.Interfaces;
using backend.Application.Mappings;
using backend.Domain.Exceptions;
using backend.Domain.Models;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class ServerService : IServerService
    {
        private readonly IServerRepository _repo;
        private readonly ILogger<ServerService> _logger;

        public ServerService(IServerRepository repo, ILogger<ServerService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<List<ServerResponseDto>> GetAllServersAsync()
        {
            var servers = await _repo.GetAllAsync();
            return servers.Select(s => s.ToDto())
                .ToList();
        }

        public async Task<ServerResponseDto> GetServerByIdAsync(int id)
        {
            var server = await _repo.GetByIdAsync(id);

            if (server == null)
                throw new NotFoundException("Сервер", id);

            return server.ToDto();
        }

        public async Task<ServerResponseDto> CreateServerAsync(CreateServerDto dto)
        {
            var server = new Server
            {
                Title = "UPDATING...",
                Map = "UPDATING...",
                Online = 0,
                MaxOnline = 0,
                IpAddress = dto.IpAddress,
                Port = dto.Port,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(server);
            _logger.LogInformation("Добавление нового сервера: {ip}:{port}", dto.IpAddress, dto.Port);
            return server.ToDto();
        }

        public async Task<bool> RemoveServerAsync(int id)
        {
            var server = await _repo.GetByIdAsync(id);

            if (server == null)
                throw new NotFoundException("Сервер", id);

            await _repo.RemoveAsync(server);
            _logger.LogInformation("Удаление сервера: {ip}:{port}", server.IpAddress, server.Port);
            return true;
        }
    }
}

