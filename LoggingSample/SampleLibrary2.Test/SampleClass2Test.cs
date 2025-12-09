using Microsoft.Extensions.Logging;
using Xunit;

namespace SampleLibrary2.Test;

public class SampleClass2Test
{
    public SampleClass2Test()
    {
        var factory = LoggerFactory.Create(x => x
            .SetMinimumLevel(LogLevel.Trace)
            .AddXunit()
            .AddDebug());
        Logging.Log.Init(factory);
    }

    [Fact]
    public void SimulateLogTest()
    {
        var x = new SampleClass2();
        x.SimulateLogTrace("Bill");
        x.SimulateLogInfo(3, 2);
        x.SimulateLogWarn();
        x.SimulateLogError();
    }
}