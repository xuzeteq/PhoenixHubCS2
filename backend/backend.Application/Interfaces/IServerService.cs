using backend.Application.Dtos.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace backend.Application.Interfaces
{
    public interface IServerService
    {
        Task<List<ServerResponseDto>> GetAllServersAsync();
        Task<ServerResponseDto> GetServerByIdAsync(int id);
        Task<ServerResponseDto> CreateServerAsync(CreateServerDto dto);
        Task<bool> RemoveServerAsync(int id);
    }
}
