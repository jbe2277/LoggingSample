using Microsoft.Extensions.Logging;
using SampleLibrary2.Logging;

namespace SampleLibrary2;

public class SampleClass
{
    public void SimulateLogTrace(string text) => Log.Default.TraceMessage(text);

    public void SimulateLogInfo(int a, int b) 
    {
        if (Log.Default.IsEnabled(LogLevel.Information))
        {
            var c = a * b;
            Log.Default.InfoMessage(c);
        }
    }

    public void SimulateLogWarn() => Log.Default.WarnMessage();

    public void SimulateLogError() => Log.Default.ErrorMessage();
}
