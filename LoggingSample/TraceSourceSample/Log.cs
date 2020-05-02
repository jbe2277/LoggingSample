using System.Diagnostics;

namespace TraceSourceSample
{
    public static class Log
    {
        public static TraceSource Default { get; } = new TraceSource("TraceSourceSample");
    }
}
