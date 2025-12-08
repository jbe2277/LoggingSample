using LoggingSampleShared;
using Microsoft.Extensions.Logging;
using System.Waf.Applications;
using System.Windows;

namespace MicrosoftLoggingSample
{
    public partial class App : Application
    {
        public App()
        {
            using var loggerFactory = LoggerFactory.Create(x =>
            {
                x.AddFilter("SampleLibrary", LogLevel.Trace)
                 .AddFilter(SampleLibrary2.Logging.Log.CategoryName, LogLevel.Trace)
                 .AddFilter(MicrosoftLoggingSample.Logger.CategoryName, LogLevel.Trace)
                 .AddDebug();
            });

            MicrosoftLoggingSample.Logger.Init(loggerFactory);
            LoggerHelper.ConfigureTraceSource(SampleLibrary.Logging.Log.Default, loggerFactory);
            SampleLibrary2.Logging.Log.Init(loggerFactory);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Logger.Default.AppStarting(ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion);
            new MainWindow("", "See Visual Studio Output View during Debugging").Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Logger.Default.AppClosed(ApplicationInfo.ProductName);
            base.OnExit(e);
        }
    }
}
