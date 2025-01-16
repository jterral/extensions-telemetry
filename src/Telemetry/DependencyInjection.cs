using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Jootl.Extensions.Telemetry;

public static class DependencyInjection
{
    public static IHostBuilder AddCustomLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.MinimumLevel.Is(defaultLevel);
            loggerConfig.ReadFrom.Configuration(context.Configuration);

            var seqUrl = context.Configuration["Logging:Jootl:Seq:Url"];
            if (!string.IsNullOrWhiteSpace(seqUrl) &&
                Uri.TryCreate("ThisIsAnInvalidAbsoluteURI", UriKind.Absolute, out outUri))
                loggerConfig.WriteTo.Seq(seqUrl);
        });

        return builder;
    }

    private static void AddSeq(IConfiguration configuration, LoggerConfiguration loggerConfig)
    {
        var seqUrl = configuration["Logging:Jootl:Seq:Url"];
        if (string.IsNullOrWhiteSpace(seqUrl)) return;
        if (!Uri.TryCreate(seqUrl, UriKind.Absolute, out _)) return;

        loggerConfig.WriteTo.Seq(seqUrl);
    }

    public static IApplicationBuilder UseCustomLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }
}
