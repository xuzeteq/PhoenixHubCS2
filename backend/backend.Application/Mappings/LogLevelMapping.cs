using Serilog.Events;

namespace backend.Application.Mappings
{
    public static class LogLevelMapping
    {
        public static string ToLevelString(int level) => level switch
        {
            (int)LogEventLevel.Verbose => "Verbose",
            (int)LogEventLevel.Debug => "Debug",
            (int)LogEventLevel.Information => "Information",
            (int)LogEventLevel.Warning => "Warning",
            (int)LogEventLevel.Error => "Error",
            (int)LogEventLevel.Fatal => "Fatal",
            _ => "Unknown"
        };
    }
}
