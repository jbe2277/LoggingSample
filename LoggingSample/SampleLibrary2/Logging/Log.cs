using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace SampleLibrary2.Logging;

public static class Log
{
    public static ILogger Default { get; private set; } = NullLogger.Instance;

    public static void Init(ILoggerFactory factory) => Default = factory.CreateLogger("SampleLibrary2");
}
