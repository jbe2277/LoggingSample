using SampleLibrary.Logging;
using System.Waf.Foundation;

namespace SampleLibrary
{
    public class SampleClass
    {
        public void SimulateLogTrace(string text) => Log.Default.Trace("Trace message: {0}", text);

        public void SimulateLogInfo(int a, int b) 
        {
            if (Log.Default.IsInfoEnabled())
            {
                var c = a * b;
                Log.Default.Info("Info message: {0}", c);
            }
        }

        public void SimulateLogWarn() => Log.Default.Warn("Warn message");

        public void SimulateLogError() => Log.Default.Error("Error message");
    }
}
