using backend.Application.Interfaces;
using backend.Infrastructure.Auth;
using backend.Infrastructure.BackgroundServices;
using backend.Infrastructure.Data;
using backend.Infrastructure.Persistence;
using backend.Infrastructure.Repositories;
using backend.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IServerRepository, ServerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPromocodeRepository, PromocodeRepository>();
            services.AddScoped<ISubscribtionRepository, SubscribtionRepository>();
            services.AddScoped<IPrivilegeRepository, PrivilegeRepository>();
            services.AddScoped<IFeatureRepository, FeatureRepository>();
            services.AddScoped<IBalanceTransactionRepository, BalanceTransactionRepository>();

            services.AddScoped<AuditLogRepository>();
            services.AddScoped<IAuditLogsService, AuditLogsService>();

            services.AddScoped<LogRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<ServerUpdateDataService>();
            services.AddHostedService<ServerBackgroundService>();
            services.AddHostedService<CleanSubscribeBackgroundService>();

            return services;
        }
    }
}
