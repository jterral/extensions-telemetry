using System.Reflection;
using Jootl.Extensions.Telemetry.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Jootl.Extensions.Telemetry.Configuration;

internal static class LoggingConfiguration
{
    private const string KeyPrefix = "Logging:Jootl";

    internal static void AddCustomEnrichments(this LoggerConfiguration loggerConfig, IConfiguration configuration)
    {
        var appName = configuration.GetValue($"{KeyPrefix}:ApplicationName", "Jootl");
        var appVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.0.0-unknown";

        loggerConfig.Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", appName)
            .Enrich.WithProperty("ApplicationVersion", appVersion);
    }

    internal static void AddHttpRequestEnrichments(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        var remoteIpAddress = httpContext.Connection.RemoteIpAddress?.ToString();
        if (!string.IsNullOrWhiteSpace(remoteIpAddress))
            diagnosticContext.Set("ClientIP", remoteIpAddress);

        var userAgent = httpContext.Request.Headers.UserAgent.ToString();
        if (!string.IsNullOrWhiteSpace(userAgent))
            diagnosticContext.Set("UserAgent", userAgent);

        var correlationId = httpContext.Request.Headers["X-Correlation-ID"].ToString();
        if (!string.IsNullOrWhiteSpace(correlationId))
            diagnosticContext.Set("CorrelationId", correlationId);
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
        var seqUrl = configuration.GetValue<string?>($"{KeyPrefix}:Seq:ApiUrl", null);
        if (string.IsNullOrWhiteSpace(seqUrl))
        {
            Console.WriteLine("Seq URL is not configured. Seq logging will not be enabled.");
            return;
        }

        if (!Uri.TryCreate(seqUrl, UriKind.Absolute, out _))
        {
            Console.WriteLine("Seq URL is not a valid URL.");
            return;
        }

        // Try to add Seq ApiKey (optional)
        var seqApiKey = configuration.GetValue<string?>($"{KeyPrefix}:Seq:ApiKey", null);
        if (string.IsNullOrWhiteSpace(seqApiKey)) Console.WriteLine("Seq is configured without ApiKey.");

        loggerConfig.WriteTo.Seq(seqUrl, apiKey: seqApiKey);
    }
}
