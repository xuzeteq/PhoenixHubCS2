using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace backend.Infrastructure.BackgroundServices
{
    public class ServerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scope;
        private readonly ILogger<ServerBackgroundService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

        public ServerBackgroundService(IServiceScopeFactory scope, ILogger<ServerBackgroundService> logger)
        {
            _scope = scope;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scope.CreateScope();
                    var updateService = scope.ServiceProvider.GetRequiredService<ServerUpdateDataService>();
                    var sw = Stopwatch.StartNew();

                    await updateService.GetAndUpdateServerDataAsync(stoppingToken);
                    sw.Stop();
                    _logger.LogInformation("Данные серверов успешно обновлены.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
