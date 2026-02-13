using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace Mishka
{
    public static class StartupManager
    {
        private const string RUN_KEY = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string APP_NAME = "Mishka";
        
        public static bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_KEY, false))
                {
                    if (key != null)
                    {
                        string value = key.GetValue(APP_NAME) as string;
                        return !string.IsNullOrEmpty(value);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking startup: {ex.Message}");
            }
            return false;
        }
        
        public static void EnableStartup()
        {
            try
            {
                string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                // For single-file publish, get the .exe path
                if (executablePath.EndsWith(".dll"))
                {
                    executablePath = executablePath.Replace(".dll", ".exe");
                }
                
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_KEY, true))
                {
                    key?.SetValue(APP_NAME, executablePath);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to enable startup: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public static void DisableStartup()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_KEY, true))
                {
                    key?.DeleteValue(APP_NAME, false);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to disable startup: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}