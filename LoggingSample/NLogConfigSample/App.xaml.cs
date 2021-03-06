﻿using LoggingSampleShared;
using NLog;
using NLog.Targets;
using System;
using System.IO;
using System.Waf.Applications;
using System.Windows;

namespace NLogConfigSample
{
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
