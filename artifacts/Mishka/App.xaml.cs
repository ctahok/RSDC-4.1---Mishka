using System;
using System.Windows;
using WMB = System.Windows.MessageBox;

namespace Mishka
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Add global exception handlers
            AppDomain.CurrentDomain.UnhandledException += (s, ev) => LogException(ev.ExceptionObject as Exception, "AppDomain");
            this.DispatcherUnhandledException += (s, ev) => 
            {
                LogException(ev.Exception, "Dispatcher");
                ev.Handled = true;
            };

            try
            {
                // First, validate runtime before any other initialization
                if (!RuntimeDetector.ValidateRuntime())
                {
                    Shutdown(1);
                    return;
                }

                // Check for tray icon support
                if (!ValidateTrayIconSupport())
                {
                    ShowFallbackMessage();
                }

                // Continue with normal startup
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                LogException(ex, "Startup");
                Shutdown(1);
            }
        }

        private void LogException(Exception? ex, string source)
        {
            string message = ex?.ToString() ?? "Unknown error";
            System.Diagnostics.Debug.WriteLine($"CRASH [{source}]: {message}");
            
            try
            {
                WMB.Show(
                    $"A critical error occurred ({source}):\n\n{ex?.Message}\n\nStack Trace:\n{ex?.StackTrace}",
                    "Critical Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                
                // Also write to a file in %APPDATA%
                string logDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mishka");
                System.IO.Directory.CreateDirectory(logDir);
                System.IO.File.AppendAllText(System.IO.Path.Combine(logDir, "crash.log"), 
                    $"\n\n[{DateTime.Now}] [{source}]\n{message}");
            }
            catch { }
        }

        private bool ValidateTrayIconSupport()
        {
            try
            {
                // Test if we can create a NotifyIcon - this tests Windows Forms integration
                using (var testIcon = new System.Windows.Forms.NotifyIcon())
                {
                    // If we can create it, Windows Forms integration works
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Tray icon validation failed: {ex.Message}");
                return false;
            }
        }

        private void ShowFallbackMessage()
        {
            try
            {
                WMB.Show(
                    "System tray functionality is not available.\n\n" +
                    "Mishka will run as a regular window application.\n\n" +
                    "Use the hotkey Ctrl+Shift+J to show/hide the main window.",
                    "Tray Icon Unavailable",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cannot show fallback message: {ex.Message}");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Clean up resources
            base.OnExit(e);
        }
    }
}