using backend.Application.Interfaces;
using backend.Domain.Models;
using CodeLogic.Core.Logging;
using Microsoft.Extensions.Logging;
using OpenGSQ.Protocols;
using System.Diagnostics;

namespace backend.Infrastructure.BackgroundServices
{
    public class ServerUpdateDataService
    {
        private readonly IServerRepository _repo;
        private readonly SemaphoreSlim _semaphore = new(10);
        private readonly ILogger<ServerUpdateDataService> _logger;

        public ServerUpdateDataService(IServerRepository repo, ILogger<ServerUpdateDataService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task GetAndUpdateServerDataAsync(CancellationToken cts)
        {
            var sw = Stopwatch.StartNew();

            var servers = await _repo.GetAllAsync();

            if (servers.Count <= 0)
                return;

            var tasks = servers.Select(s => PollAndUpdateServerAsync(s, cts));
            await Task.WhenAll(tasks);

            try
            {
                await _repo.UpdateBatchAsync(servers, cts);
                sw.Stop();
            } 
            catch (Exception ex)
            {
                _logger.LogError("Ошибка обновления серверов: {ex}", ex.Message);
            }
        }

        private async Task PollAndUpdateServerAsync(Server server, CancellationToken cts)
        {
            await _semaphore.WaitAsync(cts);

            try
            {
                var pingSw = Stopwatch.StartNew();
                var query = new Source(server.IpAddress, server.Port);
                var info = await query.GetInfo();

                pingSw.Stop();

                if (info == null)
                {
                    server.IsActive = false;
                    server.Map = server.Map ?? "Unknown";
                    server.Online = 0;
                    server.MaxOnline = 0;
                    server.Title = server.Title ?? "Unknown";
                }

                else
                {
                    server.IsActive = true;
                    server.Title = info.Name;
                    server.Map = info.Map;
                    server.Online = info.Players;
                    server.MaxOnline = info.MaxPlayers;
                }

                server.UpdatedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка опроса сервера {server.Title}: {ex.Message}");
                server.IsActive = false;
                server.UpdatedAt = DateTime.UtcNow;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
