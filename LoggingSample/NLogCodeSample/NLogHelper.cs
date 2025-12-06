using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.Diagnostics;
using LogLevel = NLog.LogLevel;

namespace LoggingSampleShared;

internal static class NLogHelper
{
    public static SourceLevels ToSourceLevels(this LogLevel level)
    {
        if (level == LogLevel.Trace) return SourceLevels.Verbose;
        if (level == LogLevel.Debug) return SourceLevels.Verbose;
        if (level == LogLevel.Info) return SourceLevels.Information;
        if (level == LogLevel.Warn) return SourceLevels.Warning;
        if (level == LogLevel.Error) return SourceLevels.Error;
        if (level == LogLevel.Fatal) return SourceLevels.Critical;
        return SourceLevels.Off;
    }

    public static void ConfigureTraceSource(TraceSource traceSource)
    {
        var config = LogManager.Configuration ?? throw new InvalidOperationException("LogManager.Configuration is null");
        traceSource.Listeners.Clear();
        traceSource.Listeners.Add(new NLogTraceListener());
        var rule = config.LoggingRules.FirstOrDefault(x => x.LoggerNamePattern == traceSource.Name) ?? throw new NotSupportedException(traceSource.Name);
        traceSource.Switch.Level = rule.Levels.Min()?.ToSourceLevels() ?? SourceLevels.Off;
    }

    public static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
        {
            var configuration = LogManager.Configuration ?? throw new InvalidOperationException("LogManager.Configuration is null");
            foreach (var x in configuration.LoggingRules)
            {
                builder.AddFilter(x.LoggerNamePattern, x.Levels.Min()?.ToMSLogLevel() ?? Microsoft.Extensions.Logging.LogLevel.None);
            }
            builder.AddNLog();
        });
    }

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
