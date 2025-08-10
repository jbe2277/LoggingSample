using NLog;

namespace LoggingSampleShared;

internal static class NLogHelper
{
    public static Microsoft.Extensions.Logging.LogLevel ToMSLogLevel(this LogLevel level)
    {
        if (level == LogLevel.Trace) return Microsoft.Extensions.Logging.LogLevel.Trace;
        if (level == LogLevel.Debug) return Microsoft.Extensions.Logging.LogLevel.Debug;
        if (level == LogLevel.Info) return Microsoft.Extensions.Logging.LogLevel.Information;
        if (level == LogLevel.Warn) return Microsoft.Extensions.Logging.LogLevel.Warning;
        if (level == LogLevel.Error) return Microsoft.Extensions.Logging.LogLevel.Error;
        if (level == LogLevel.Fatal) return Microsoft.Extensions.Logging.LogLevel.Critical;
        return Microsoft.Extensions.Logging.LogLevel.None;
    }
}
