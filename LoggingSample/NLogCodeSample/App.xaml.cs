using LoggingSampleShared;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Waf.Applications;
using System.Windows;

namespace NLogCodeSample
{
    public partial class App : Application
    {
        private static readonly (string loggerNamePattern, LogLevel minLevel)[] logSettings =
        {
            ("NLogCodeSample", LogLevel.Trace),
            ("SampleLibrary", LogLevel.Trace),  // Use Trace here; this is controlled by TraceSource SwitchLevel
        };

        private readonly string logFolder;
        private readonly string logFileName;

        public App()
        {
            logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName, "Log")
                + Path.DirectorySeparatorChar;
            logFileName = "App.log";

            SampleLibrary.Logging.Log.Default.Listeners.Clear();
            SampleLibrary.Logging.Log.Default.Listeners.Add(new NLogTraceListener());
            SampleLibrary.Logging.Log.Default.Switch.Level = SourceLevels.Verbose;

            var fileTarget = new FileTarget("fileTarget")
            {
                FileName = Path.Combine(logFolder, logFileName),
                Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.ff} [${level:format=FirstCharacter}] ${logger} ${message}  ${exception:format=tostring}",
                ArchiveAboveSize = 5_000_000,  // 5 MB
                MaxArchiveFiles = 2,
            };
            var traceTarget = new TraceTarget("traceTarget") { Layout = fileTarget.Layout, RawWrite = true };
            var logConfig = new LoggingConfiguration();
            logConfig.DefaultCultureInfo = CultureInfo.InvariantCulture;
            logConfig.AddTarget(fileTarget);
            logConfig.AddTarget(traceTarget);
            var maxLevel = LogLevel.AllLoggingLevels.Last();
            foreach (var (loggerNamePattern, minLevel) in logSettings)
            {
                logConfig.AddRule(minLevel, maxLevel, fileTarget, loggerNamePattern);
                logConfig.AddRule(minLevel, maxLevel, traceTarget, loggerNamePattern);
            }
            LogManager.Configuration = logConfig;
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
            base.OnExit(e);
        }
    }
}
