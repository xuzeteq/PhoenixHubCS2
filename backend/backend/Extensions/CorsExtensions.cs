using backend.Application.Options;

namespace backend.API.Extensions
{
    public static class CorsExtensions
    {
        public const string FrontendPolicy = "AllowFrontend";

        public static IServiceCollection AddFrontendCors(this IServiceCollection services, IConfiguration configuration)
        {
            var frontendUrl = configuration.GetSection(FrontendOptions.SectionName).Get<FrontendOptions>()?.Url
                              ?? "http://localhost:3000";

            services.AddCors(options =>
            {
                options.AddPolicy(FrontendPolicy, policy =>
                {
                    policy.WithOrigins(frontendUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
