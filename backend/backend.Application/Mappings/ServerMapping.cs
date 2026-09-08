using backend.Application.Dtos.Server;
using backend.Domain.Models;

namespace backend.Application.Mappings
{
    public static class ServerMapping
    {
        public static ServerResponseDto ToDto(this Server server)
        {
            return new ServerResponseDto
            {
                Id = server.Id,
                Title = server.Title ?? "Unknown",
                Map = server.Map ?? "Unknown",
                IpAddress = server.IpAddress,
                Port = server.Port,
                Online = server.Online ?? 0,
                MaxOnline = server.MaxOnline ?? 0,
                IsActive = server.IsActive ?? false,
                CreatedAt = server.CreatedAt ,
                UpdatedAt = server.UpdatedAt ?? DateTime.UtcNow,
            };
        }
    }
}
