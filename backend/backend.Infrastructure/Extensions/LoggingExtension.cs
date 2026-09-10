using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace backend.Infrastructure.Extensions
{
    public static class LoggingExtension
    {
        public static void LoggingSerilog(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            var columnOptions = new Dictionary<string, ColumnWriterBase>
            {
                { "message", new RenderedMessageColumnWriter() },
                { "message_template", new MessageTemplateColumnWriter()  },
                { "level", new LevelColumnWriter() },
                { "timestamp", new TimestampColumnWriter() },
                { "exception",  new ExceptionColumnWriter() },
                { "properties", new LogEventSerializedColumnWriter() },
            };

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
                .WriteTo.PostgreSQL(
                    connectionString: connectionString,
                    tableName: "logs",
                    columnOptions: columnOptions,
                    needAutoCreateTable: true,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            builder.Services.AddSerilog(Log.Logger);
            builder.Host.UseSerilog();
        }
    }
}
