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
                 .AddFilter("MicrosoftLoggingSample", LogLevel.Trace)
                 .AddDebug();
            });

            MicrosoftLoggingSample.Log.Init(loggerFactory);
            LoggerHelper.ConfigureTraceSource(SampleLibrary.Logging.Log.Default, loggerFactory);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Log.Default.LogInformation("{productName} {version} is starting; OS: {osVersion}", ApplicationInfo.ProductName, ApplicationInfo.Version, Environment.OSVersion);
            new MainWindow("", "See Visual Studio Output View during Debugging").Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Default.LogInformation("{productName} closed", ApplicationInfo.ProductName);
            base.OnExit(e);
        }
    }
}
