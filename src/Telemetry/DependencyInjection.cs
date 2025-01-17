using Jootl.Extensions.Telemetry.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Jootl.Extensions.Telemetry;

public static class DependencyInjection
{
    private const string KeyPrefix = "Logging:Jootl";

    public static IHostBuilder AddCustomLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.AddCustomLogLevels(context.Configuration);
            loggerConfig.AddCustomEnrichments(context.Configuration);
            loggerConfig.AddCustomSinks(context.Configuration);
        });

        return builder;
    }

    public static IApplicationBuilder UseCustomLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(options =>
            options.EnrichDiagnosticContext = LoggingConfiguration.AddHttpRequestEnrichments);

        return app;
    }
}
