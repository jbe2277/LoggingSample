using System;
using System.Diagnostics;
using System.IO;
using System.Waf.Applications;
using System.Windows;

namespace TraceSourceSample
{
    public partial class App : Application
    {
        private readonly string logFolder;
        private readonly string logFileName;
        private readonly TextWriterTraceListener fileTraceListener;

        public App()
        {
            logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName, "Log")
                + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(logFolder);
            logFileName = "App.log";
            fileTraceListener = new TextWriterTraceListener(Path.Combine(logFolder, logFileName));
            
            // Configure logging
            SampleLibrary.Logging.Log.Default.Listeners.Add(fileTraceListener);
            SampleLibrary.Logging.Log.Default.Switch.Level = SourceLevels.Verbose;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainWindow(logFolder, logFileName).Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            fileTraceListener.Dispose();
            base.OnExit(e);
        }
    }
}
