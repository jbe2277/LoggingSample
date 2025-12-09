using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Xunit;

namespace SampleLibrary2.Test;

public static class XunitLoggerFactoryExtensions
{
    public static ILoggingBuilder AddXunit(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, XunitLoggerProvider>());
        return builder;
    }
}

public sealed class XunitLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, XunitLogger> loggers = new();

    public ILogger CreateLogger(string categoryName) => loggers.GetOrAdd(categoryName, name => new(TestContext.Current.TestOutputHelper, name));

    public void Dispose() { }


    private sealed class XunitLogger(ITestOutputHelper? output, string category) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (output is null || !IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            var line = $"{DateTimeOffset.Now:HH:mm:ss.fff} [{FormatLevel(logLevel)}] {category}: {message}";
            if (exception is not null) line += Environment.NewLine + exception;

            try { output.WriteLine(line); }
            catch (InvalidOperationException) { /* Output helper isn't available after test completion; ignore. */ }
        }

        private static string FormatLevel(LogLevel level) => level switch
        {
            LogLevel.Trace => "T",
            LogLevel.Debug => "D",
            LogLevel.Information => "I",
            LogLevel.Warning => "W",
            LogLevel.Error => "E",
            LogLevel.Critical => "C",
            _ => ""
        };
    }
}

