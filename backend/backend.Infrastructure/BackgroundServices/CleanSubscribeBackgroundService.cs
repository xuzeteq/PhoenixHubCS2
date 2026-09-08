using backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.BackgroundServices
{
    public class CleanSubscribeBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CleanSubscribeBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromDays(1);

        public CleanSubscribeBackgroundService(IServiceProvider serviceProvider,
            ILogger<CleanSubscribeBackgroundService> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Сервис очистки просроченных подписок запущен.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var cleanupService = scope.ServiceProvider.GetRequiredService<SubscribtionsCleanupService>();
                    await cleanupService.CleanupSubscribtions(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка сервиса очистки просроченных подписок.");
                }

                await Task.Delay(_checkInterval);
            }

            
        }
    }
}
