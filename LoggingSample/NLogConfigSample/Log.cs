using NLog;

namespace NLogConfigSample
{
    internal static class Log
    {
        public static Logger Default { get; } = LogManager.GetLogger("NLogConfigSample");
    }
}
