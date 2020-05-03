using LoggingSampleShared;
using NLog;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.IO;
using System.Waf.Applications;
using System.Windows;

namespace NLogConfigSample
{
    public partial class App : Application
    {
        public App()
        {
            SampleLibrary.Logging.Log.Default.Listeners.Clear();
            SampleLibrary.Logging.Log.Default.Listeners.Add(new NLogTraceListener());
            SampleLibrary.Logging.Log.Default.Switch.Level = SourceLevels.Verbose;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Log.Default.Info("{0} {1} is starting; OS: {2}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion);
            string fileName = ((FileTarget)LogManager.Configuration.FindTargetByName("fileTarget")).FileName.Render(new LogEventInfo());
            var logFolder = Path.GetDirectoryName(fileName);
            var logFileName = Path.GetFileName(fileName);
            new MainWindow(logFolder, logFileName).Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Default.Info("{0} closed", ApplicationInfo.ProductName);
            base.OnExit(e);
        }
    }
}
