using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace backend.Infrastructure.Extensions
{
    public static class LoggingExtension
    {
        public static void LoggingSerilog(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    "logs/app.log",
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                    fileSizeLimitBytes: 10_000_000,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Services.AddSerilog(Log.Logger);
            builder.Host.UseSerilog();
        }
    }
}
