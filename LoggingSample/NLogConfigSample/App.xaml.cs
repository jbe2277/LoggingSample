using LoggingSampleShared;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.IO;
using System.Waf.Applications;
using System.Windows;

namespace NLogConfigSample;

public partial class App : Application
{
    public App()
    {
        NLogHelper.ConfigureTraceSource(SampleLibrary.Logging.Log.Default);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Log.Default.Info("{0} {1} is starting; OS: {2}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion);
        
        // Read fileName from config and show it in the UI
        string fileName = ((FileTarget)((AsyncTargetWrapper)LogManager.Configuration.FindTargetByName("fileTarget")).WrappedTarget).FileName.Render(new LogEventInfo());
        var logFolder = Path.GetDirectoryName(fileName) ?? throw new InvalidOperationException("Could not get the directory name from the log file name.");
        var logFileName = Path.GetFileName(fileName);
        
        new MainWindow(logFolder, logFileName).Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Default.Info("{0} closed", ApplicationInfo.ProductName);
        base.OnExit(e);
    }
}
