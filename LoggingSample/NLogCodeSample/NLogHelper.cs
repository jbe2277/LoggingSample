using NLog;
using System.Diagnostics;

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
        traceSource.Listeners.Clear();
        traceSource.Listeners.Add(new NLogTraceListener());
        var rule = LogManager.Configuration.LoggingRules.FirstOrDefault(x => x.LoggerNamePattern == traceSource.Name) ?? throw new NotSupportedException(traceSource.Name);
        traceSource.Switch.Level = rule.Levels.Min()?.ToSourceLevels() ?? SourceLevels.Off;
    }
}
