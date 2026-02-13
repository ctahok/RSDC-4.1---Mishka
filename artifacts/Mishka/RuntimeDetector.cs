using System;
using System.Runtime.InteropServices;
using System.Windows;
using WMB = System.Windows.MessageBox;

namespace Mishka
{
    public static class RuntimeDetector
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private const string RUNTIME_DOWNLOAD_URL = "https://dotnet.microsoft.com/download/dotnet/8.0";
        
        public static bool ValidateRuntime()
        {
            try
            {
                // Check if .NET 8 runtime is available by looking for key runtime files
                string[] requiredDlls = 
                {
                    "hostfxr.dll",
                    "hostpolicy.dll", 
                    "coreclr.dll"
                };

                foreach (string dll in requiredDlls)
                {
                    IntPtr handle = LoadLibrary(dll);
                    if (handle == IntPtr.Zero)
                    {
                        ShowRuntimeError($"Required runtime component '{dll}' not found");
                        return false;
                    }
                    FreeLibrary(handle);
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowRuntimeError($"Runtime validation failed: {ex.Message}");
                return false;
            }
        }

        private static void ShowRuntimeError(string details)
        {
            try
            {
            WMB.Show(
                    $"Mishka requires .NET 8 Desktop Runtime to run.\n\n" +
                    $"Please install .NET 8 Desktop Runtime from:\n\n" +
                    $"{RUNTIME_DOWNLOAD_URL}\n\n" +
                    $"Error details: {details}",
                    ".NET Runtime Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
            );
            }
            catch
            {
                // If MessageBox fails, we're in a really bad state
                // This can happen if the runtime is so broken that WPF can't initialize
                System.Diagnostics.Debug.WriteLine($"FATAL: Runtime error and cannot show message box: {details}");
            }
        }
    }
}