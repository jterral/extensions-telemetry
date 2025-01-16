using System.ComponentModel.DataAnnotations;

namespace Jootl.Extensions.Telemetry.Configuration;

public sealed class SeqOptions
{
    [Required] public required Uri BaseAddress { get; init; }
}
