using Microsoft.Extensions.Logging;

namespace MicrosoftLoggingSample;

internal static class Log
{
    public static ILogger Default { get; private set; } = null!;

    public static void Init(ILoggerFactory factory) => Default = factory.CreateLogger("MicrosoftLoggingSample");
}