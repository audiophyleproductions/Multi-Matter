using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace USBFormatter;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
            LogError("Unhandled application error", eventArgs.ExceptionObject as Exception);
        TaskScheduler.UnobservedTaskException += (_, eventArgs) =>
        {
            LogError("Unobserved task error", eventArgs.Exception);
            eventArgs.SetObserved();
        };

        try
        {
            var window = new MainWindow();
            MainWindow = window;
            window.Show();
        }
        catch (Exception ex)
        {
            ShowStartupFailure("Multi-Matter could not open its main window.", ex);
            Shutdown(1);
        }
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        ShowStartupFailure("Multi-Matter encountered an unexpected error.", e.Exception);
        e.Handled = true;
        Shutdown(1);
    }

    private static string LogError(string reason, Exception? exception)
    {
        string location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Multi-Matter");
        try
        {
            Directory.CreateDirectory(location);
            string path = Path.Combine(location, "startup-error.log");
            File.AppendAllText(path,
                $"[{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz}] {reason}{Environment.NewLine}{exception}{Environment.NewLine}{Environment.NewLine}",
                Encoding.UTF8);
            return path;
        }
        catch
        {
            return Path.Combine(location, "startup-error.log");
        }
    }

    private static void ShowStartupFailure(string reason, Exception ex)
    {
        var log = LogError(reason, ex);
        try
        {
            MessageBox.Show(reason + "\n\n" + ex.Message + "\n\nDiagnostic log: " + log,
                "Multi-Matter — startup error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch { }
    }
}
