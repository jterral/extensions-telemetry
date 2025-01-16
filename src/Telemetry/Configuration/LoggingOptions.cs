using System.ComponentModel.DataAnnotations;

namespace Jootl.Extensions.Telemetry.Configuration;

public sealed class LoggingOptions
{
    [Required] public required string ApplicationName { get; init; }

    public SeqOptions? Seq { get; init; }
}
