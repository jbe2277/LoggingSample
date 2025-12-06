using System.Diagnostics;

namespace SampleLibrary.Logging
{
    public static partial class Log
    {
        public static TraceSource Default { get; } = new TraceSource("SampleLibrary");
    }
}
