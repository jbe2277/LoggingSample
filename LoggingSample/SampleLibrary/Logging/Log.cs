using System.Diagnostics;

namespace SampleLibrary.Logging
{
    public static class Log
    {
        public static TraceSource Default { get; } = new TraceSource("SampleLibrary");
    }
}
