using System.ComponentModel.DataAnnotations;

namespace EduSource.Persistance.DependencyInjection.Options;
public record SqlServerRetryOptions
{
    [Required, Range(5, 20)]
    public int MaxRetryCount { get; set; } = 5;
    [Required, Timestamp]
    public TimeSpan MaxRetryDelay { get; set; }
    public int[]? ErrorNumbersToAdd { get; set; }
}
