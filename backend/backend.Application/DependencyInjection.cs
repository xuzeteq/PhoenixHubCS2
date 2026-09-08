using backend.Application.Interfaces;
using backend.Application.Services;
using backend.Application.Services.AdminService;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IServerService, ServerService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<ISubscribtionService, SubscribtionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPromocodeService, PromocodeService>();
            services.AddScoped<SubscribtionsCleanupService>();

            return services;
        }
    }
}
