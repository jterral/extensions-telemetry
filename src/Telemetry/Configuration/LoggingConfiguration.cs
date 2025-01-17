using Jootl.Extensions.Telemetry.Extensions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Jootl.Extensions.Telemetry.Configuration;

internal static class LoggingConfiguration
{
    private const string KeyPrefix = "Logging:Jootl";

    internal static void AddCustomEnriches(this LoggerConfiguration loggerConfig, IConfiguration configuration)
    {
        var applicationName = configuration.GetValue($"{KeyPrefix}:Application", "Jootl");
        var logVersion = typeof(DependencyInjection).Assembly.GetName().Version?.ToString() ?? "0.0.0";

        loggerConfig.Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName)
            .Enrich.WithProperty("LogVersion", logVersion);
    }

    internal static void AddCustomLogLevels(this LoggerConfiguration loggerConfig, IConfiguration configuration)
    {
        const string loggingKey = "Logging:LogLevel";

        loggerConfig.MinimumLevel.Is(LogEventLevel.Debug);

        var logLevels = configuration.GetSection(loggingKey)
            .GetChildren()
            .Where(x => !string.IsNullOrWhiteSpace(x.Value))
            .ToDictionary(x => x.Key, x => x.Value!.ToLogEventLevel());

        foreach (var (key, value) in logLevels)
        {
            if (key.Equals("Default", StringComparison.OrdinalIgnoreCase))
            {
                loggerConfig.MinimumLevel.Is(value);
                continue;
            }

            loggerConfig.MinimumLevel.Override(key, value);
        }
    }

    internal static void AddCustomSinks(this LoggerConfiguration loggerConfig, IConfiguration configuration)
    {
        // Always add console sink
        loggerConfig.WriteTo.Console();

        // Try to add Seq sink
        var seqUrl = configuration.GetValue($"{KeyPrefix}:Seq:Url", string.Empty);
        if (string.IsNullOrWhiteSpace(seqUrl))
        {
            Log.Information("Seq is not configured. Seq logging will not be enabled.");
            // Console.WriteLine("Seq URL is not configured. Seq logging will not be enabled.");
            return;
        }

        if (!Uri.TryCreate(seqUrl, UriKind.Absolute, out _))
        {
            Log.Warning("Seq address '{SeqUrl}' is not a valid URL.", seqUrl);
            // Console.WriteLine("Seq URL is not a valid URL.");
            return;
        }

        loggerConfig.WriteTo.Seq(seqUrl);
    }
}
