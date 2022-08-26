using LoggingSampleShared;
using System.Diagnostics;
using System.IO;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows;

namespace TraceSourceSample;

public partial class App : Application
{
    private readonly string logFolder;
    private readonly string logFileName;
    private readonly TextWriterTraceListener fileTraceListener;

    public App()
    {
        logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName, "Log") + Path.DirectorySeparatorChar;
        logFileName = "App.log";
        Directory.CreateDirectory(logFolder);
        fileTraceListener = new TextWriterTraceListener(Path.Combine(logFolder, logFileName));
        
        // Configure logging
        SampleLibrary.Logging.Log.Default.Switch.Level = SourceLevels.Verbose;
        TraceSourceSample.Log.Default.Switch.Level = SourceLevels.Verbose;
        
        SampleLibrary.Logging.Log.Default.Listeners.Add(fileTraceListener);
        TraceSourceSample.Log.Default.Listeners.Add(fileTraceListener);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Log.Default.Info("{0} {1} is starting; OS: {2}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion);
        new MainWindow(logFolder, logFileName).Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Default.Info("{0} closed", ApplicationInfo.ProductName);
        fileTraceListener.Dispose();
        base.OnExit(e);
    }
}
