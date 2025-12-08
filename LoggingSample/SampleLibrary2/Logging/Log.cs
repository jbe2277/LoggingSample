using Microsoft.Extensions.Logging;

namespace SampleLibrary2.Logging;

/// <summary>A log facade that enables the initialisation of the logger.</summary>
public static class Log
{
    /// <summary>The category name "SampleLibrary2" used by this library for logging.</summary>
    public const string CategoryName = "SampleLibrary2";

    /// <summary>Initialize the logger.</summary>
    /// <param name="factory">The logger factory.</param>
    public static void Init(ILoggerFactory factory) => Logger.Default = factory.CreateLogger(CategoryName);
}
