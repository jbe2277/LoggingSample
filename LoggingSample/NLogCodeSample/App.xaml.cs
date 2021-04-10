using LoggingSampleShared;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Globalization;
using System.IO;
using System.Waf.Applications;
using System.Windows;

namespace NLogCodeSample
{
    public partial class App : Application
    {
        private static readonly (string loggerNamePattern, LogLevel minLevel)[] logSettings =
        {
            ("NLogCodeSample", LogLevel.Trace),
            ("SampleLibrary", LogLevel.Trace),
        };

        private readonly string logFolder;
        private readonly string logFileName;

        public App()
        {
            logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName, "Log")
                + Path.DirectorySeparatorChar;
            logFileName = "Application.log";

            var layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.ff} [${level:format=FirstCharacter}] ${processid} ${logger} ${message}  ${exception:format=tostring}";
            var fileTarget = new AsyncTargetWrapper("fileTarget", new FileTarget()
                {
                    FileName = Path.Combine(logFolder, logFileName),
                    Layout = layout,
                    ArchiveAboveSize = 10_000,  // 10 kB ... this low size is used just for testing purpose
                    MaxArchiveFiles = 1,
                    ArchiveNumbering = ArchiveNumberingMode.Rolling
                })
            { OverflowAction = AsyncTargetWrapperOverflowAction.Block };
            var traceTarget = new AsyncTargetWrapper("traceTarget", new TraceTarget() 
                { 
                    Layout = layout, 
                    RawWrite = true 
                })
            { OverflowAction = AsyncTargetWrapperOverflowAction.Block };

            var logConfig = new LoggingConfiguration();
            logConfig.DefaultCultureInfo = CultureInfo.InvariantCulture;
            logConfig.AddTarget(fileTarget);
            logConfig.AddTarget(traceTarget);
            foreach (var (loggerNamePattern, minLevel) in logSettings)
            {
                var rule = new LoggingRule(loggerNamePattern, minLevel, fileTarget);
                rule.Targets.Add(traceTarget);
                logConfig.LoggingRules.Add(rule);
            }

            LogManager.Configuration = logConfig;
            NLogHelper.ConfigureTraceSource(SampleLibrary.Logging.Log.Default);
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
