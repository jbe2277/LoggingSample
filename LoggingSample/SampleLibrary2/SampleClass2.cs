using Microsoft.Extensions.Logging;

namespace SampleLibrary2;

public class SampleClass2
{
    public void SimulateLogTrace(string text) => Logger.Default.TraceMessage(text);

    public void SimulateLogInfo(int a, int b) 
    {
        if (Logger.Default.IsEnabled(LogLevel.Information))
        {
            var c = a * b;
            Logger.Default.InfoMessage(c);
        }
    }

    public void SimulateLogWarn() => Logger.Default.WarnMessage();

    public void SimulateLogError() => Logger.Default.ErrorMessage();
}
