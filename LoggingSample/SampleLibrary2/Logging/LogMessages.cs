using Microsoft.Extensions.Logging;

namespace SampleLibrary2.Logging;

internal static partial class LogMessages
{
    [LoggerMessage(Level = LogLevel.Trace, Message = "Trace message: {text}")]
    public static partial void TraceMessage(this ILogger logger, string text);

    [LoggerMessage(Level = LogLevel.Information, Message = "Info message: {value}")]
    public static partial void InfoMessage(this ILogger logger, int value);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Warn message")]
    public static partial void WarnMessage(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Error message")]
    public static partial void ErrorMessage(this ILogger logger);
}
