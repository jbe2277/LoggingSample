using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MicrosoftLoggingSample;

public static class LoggerHelper
{
    private static readonly (LogLevel, SourceLevels)[] levelMapping = new[]
    {
        (LogLevel.Critical, SourceLevels.Critical),
        (LogLevel.Error, SourceLevels.Error),
        (LogLevel.Warning, SourceLevels.Warning),
        (LogLevel.Information, SourceLevels.Information),
        (LogLevel.Debug, SourceLevels.Verbose),
    };

    public static void ConfigureTraceSource(TraceSource traceSource, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(traceSource.Name);
        traceSource.Switch.Level = levelMapping.LastOrDefault(x => logger.IsEnabled(x.Item1), (LogLevel.None, SourceLevels.Off)).Item2;
        var l = traceSource.Listeners;
        l.Clear();
        l.Add(new LoggerTraceListener(logger));
    }


    private sealed class LoggerTraceListener : TraceListener
    {
        private readonly ILogger logger;

        public LoggerTraceListener(ILogger logger)
        {
            this.logger = logger;
        }

        public override void Write(string? message) => logger.LogInformation(message);

        public override void WriteLine(string? message) => logger.LogInformation(message);

        public override void WriteLine(string? message, string? category) => logger.LogInformation($"{category} {message}");

        public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id)
        {
            TraceEvent(eventCache, source, eventType, id, "");
        }

        public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, object? data)
        {
            TraceEvent(eventCache, source, eventType, id, data?.ToString());
        }

        public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, params object?[]? data)
        {
            if (data is null) TraceEvent(eventCache, source, eventType, id);
            else TraceEvent(eventCache, source, eventType, id, string.Join(", ", data));
        }

        public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
        {
            if (eventType is TraceEventType.Critical) logger.LogCritical(id, source + " " + message);
            else if (eventType is TraceEventType.Error) logger.LogError(id, source + " " + message);
            else if (eventType is TraceEventType.Warning) logger.LogWarning(id, source + " " + message);
            else if (eventType is TraceEventType.Information) logger.LogInformation(id, source + " " + message);
            else if (eventType is TraceEventType.Verbose) logger.LogTrace(id, source + " " + message);
            else if (eventType is TraceEventType.Start) logger.LogInformation(id, "Start: " + source + " " + message);
            else if (eventType is TraceEventType.Stop) logger.LogInformation(id, "Stop: " + source + " " + message);
            else if (eventType is TraceEventType.Suspend) logger.LogInformation(id, "Suspend: " + source + " " + message);
            else if (eventType is TraceEventType.Resume) logger.LogInformation(id, "Resume: " + source + " " + message);
            else if (eventType is TraceEventType.Transfer) logger.LogInformation(id, "Transfer: " + source + " " + message);
            else throw new NotSupportedException(eventType.ToString());
        }

        public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? format, params object?[]? args)
        {
            if (args is null) TraceEvent(eventCache, source, eventType, id, format);
            else if (eventType is TraceEventType.Critical) logger.LogCritical(id, source + " " + format, args);
            else if (eventType is TraceEventType.Error) logger.LogError(id, source + " " + format, args);
            else if (eventType is TraceEventType.Warning) logger.LogWarning(id, source + " " + format, args);
            else if (eventType is TraceEventType.Information) logger.LogInformation(id, source + " " + format, args);
            else if (eventType is TraceEventType.Verbose) logger.LogTrace(id, source + " " + format, args);
            else if (eventType is TraceEventType.Start) logger.LogInformation(id, "Start: " + source + " " + format, args);
            else if (eventType is TraceEventType.Stop) logger.LogInformation(id, "Stop: " + source + " " + format, args);
            else if (eventType is TraceEventType.Suspend) logger.LogInformation(id, "Suspend: " + source + " " + format, args);
            else if (eventType is TraceEventType.Resume) logger.LogInformation(id, "Resume: " + source + " " + format, args);
            else if (eventType is TraceEventType.Transfer) logger.LogInformation(id, "Transfer: " + source + " " + format, args);
            else throw new NotSupportedException(eventType.ToString());
        }
    }
}

