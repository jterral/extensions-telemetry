using Serilog.Events;

namespace Jootl.Extensions.Telemetry.Extensions;

internal static class StringExtensions
{
    internal static LogEventLevel ToLogEventLevel(this string value)
    {
        return value.ToLowerInvariant() switch
        {
            "debug" => LogEventLevel.Debug,
            "information" => LogEventLevel.Information,
            "warning" => LogEventLevel.Warning,
            "error" => LogEventLevel.Error,
            "fatal" => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };
    }
}
