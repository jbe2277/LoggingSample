using Microsoft.Extensions.Logging;

namespace MicrosoftLoggingSample;

internal static partial class Log
{
    public static ILogger Default { get; private set; } = null!;

    public static void Init(ILoggerFactory factory) => Default = factory.CreateLogger("MicrosoftLoggingSample");


    [LoggerMessage(Level = LogLevel.Information, Message = "{productName} {version} is starting; OS: {osVersion}")]
    public static partial void AppStarting(this ILogger logger, string productName, string version, OperatingSystem osVersion);

    [LoggerMessage(Level = LogLevel.Information, Message = "{productName} closed")]
    public static partial void AppClosed(this ILogger logger, string productName);
}