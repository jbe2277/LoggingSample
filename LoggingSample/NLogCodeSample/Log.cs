using NLog;

namespace NLogCodeSample
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("NLogCodeSample");
    }
}
