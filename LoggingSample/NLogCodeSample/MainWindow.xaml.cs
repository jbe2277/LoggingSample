using SampleLibrary;
using System.Diagnostics;
using System.Waf.Applications;
using System.Windows;

namespace LoggingSampleShared;

public partial class MainWindow : Window
{
    private readonly SampleClass sampleObject;

    public MainWindow(string logFolder, string logFileName)
    {
        LogFolder = logFolder;
        LogFileName = logFileName;
        InitializeComponent();
        Title = ApplicationInfo.ProductName;
        sampleObject = new SampleClass();
    }

    public string LogFolder { get; }

    public string LogFileName { get; }

    private void TraceMessageClick(object sender, RoutedEventArgs e) => sampleObject.SimulateLogTrace("Bill");

    private void InfoMessageClick(object sender, RoutedEventArgs e) => sampleObject.SimulateLogInfo(5, 3);

    private void WarnMessageClick(object sender, RoutedEventArgs e) => sampleObject.SimulateLogWarn(); 

    private void ErrorMessageClick(object sender, RoutedEventArgs e) => sampleObject.SimulateLogError();

    private void ErrorMessages100Click(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < 100; i++) sampleObject.SimulateLogError();
    }

    private void LogFileClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Clipboard.SetText(LogFolder);
            Process.Start("explorer", LogFolder);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error: Could not open the log folder", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ThrowExceptionClick(object sender, RoutedEventArgs e) => throw new InvalidOperationException("Test exception");

    private async void ThrowAwaitedExceptionClick(object sender, RoutedEventArgs e)
    {
        await Task.Run(() => { throw new InvalidOperationException("Awaited test exception"); });
    }

    private void ThrowAsyncExceptionClick(object sender, RoutedEventArgs e)
    {
        Task.Run(() => { throw new InvalidOperationException("Async test exception"); });
    }

    private void RunGCClick(object sender, RoutedEventArgs e)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}
