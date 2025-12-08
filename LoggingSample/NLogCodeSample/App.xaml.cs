using LoggingSampleShared;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.Globalization;
using System.IO;
using System.Waf.Applications;
using System.Windows;

namespace NLogCodeSample;

public partial class App : Application
{
    private static readonly (string loggerNamePattern, LogLevel minLevel)[] logSettings =
    [
        ("NLogCodeSample", LogLevel.Trace),
        ("SampleLibrary", LogLevel.Trace),
        (SampleLibrary2.Logging.Log.CategoryName, LogLevel.Trace),
    ];

    private readonly string logFolder;
    private readonly string logFileName;

    public App()
    {
        logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName, "Log") + Path.DirectorySeparatorChar;
        logFileName = "Application.log";

        LogManager.Setup().LoadConfiguration(c =>
        {
            c.Configuration.DefaultCultureInfo = CultureInfo.InvariantCulture;
            var layout = "${date:universalTime=true:format=yyyy-MM-dd HH\\:mm\\:ss.ff} [${level:format=FirstCharacter}] ${processid} ${logger} ${message} ${exception}";
            var fileName = Path.Combine(logFolder, logFileName);
            var fileTarget = c.ForTarget("fileTarget").WriteTo(new AtomicFileTarget
            {
                FileName = fileName,
                Layout = layout,
                ArchiveAboveSize = 10_000,  // 10 kB ... this low size is used just for testing purpose
                MaxArchiveFiles = 2,
            }).WithAsync(AsyncTargetWrapperOverflowAction.Block);
            var traceTarget = c.ForTarget("traceTarget").WriteTo(new TraceTarget
            {
                Layout = layout,
                RawWrite = true
            }).WithAsync(AsyncTargetWrapperOverflowAction.Block);

            foreach (var (loggerNamePattern, minLevel) in logSettings)
            {
                c.ForLogger(loggerNamePattern).FilterMinLevel(minLevel).WriteTo(fileTarget).WriteTo(traceTarget);
            }
        });
        NLogHelper.ConfigureTraceSource(SampleLibrary.Logging.Log.Default);
        var loggerFactory = NLogHelper.CreateLoggerFactory();
        SampleLibrary2.Logging.Log.Init(loggerFactory);

//#if !DEBUG
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += (_, ea) => Log.Default.Warn(ea.Exception, "UnobservedTaskException");
//#endif
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
        LogManager.LogFactory.FlushAsync(CancellationToken.None).GetAwaiter().GetResult();
        base.OnExit(e);
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception ?? throw new InvalidOperationException("Unknown exception object");
        Log.Default.Error(ex, "UnhandledException; IsTerminating={0}", e.IsTerminating);
        LogManager.LogFactory.FlushAsync(CancellationToken.None).GetAwaiter().GetResult();

        var message = string.Format(CultureInfo.CurrentCulture, "Unknown application error\n\n{0}", ex);
        if (MainWindow?.IsVisible == true)
        {
            MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
        }
    }
}
