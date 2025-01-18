using Jootl.Extensions.Telemetry.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Jootl.Extensions.Telemetry;

public static class DependencyInjection
{
    private const string KeyPrefix = "Logging:Jootl";

    /// <summary>
    ///     Adds custom logging configuration to the host builder.
    /// </summary>
    /// <param name="builder">The host builder.</param>
    /// <returns>
    ///     The host builder with custom logging configuration.
    /// </returns>
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

    /// <summary>
    ///     Adds custom logging configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>
    ///     The service collection with custom logging configuration.
    /// </returns>
    public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((context, loggerConfig) =>
        {
            loggerConfig.AddCustomLogLevels(configuration);
            loggerConfig.AddCustomEnrichments(configuration);
            loggerConfig.AddCustomSinks(configuration);
        });

        return services;
    }

    /// <summary>
    ///     Adds custom logging configuration to the application builder.
    ///     This is used to enrich the log context with HTTP request information (only for ASP.NET Core applications).
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>
    ///     The application builder with custom logging configuration.
    /// </returns>
    public static IApplicationBuilder UseCustomLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(options =>
            options.EnrichDiagnosticContext = LoggingConfiguration.AddHttpRequestEnrichments);

        return app;
    }
}
