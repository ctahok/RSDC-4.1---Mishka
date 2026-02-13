using System;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using WMB = System.Windows.MessageBox;

namespace Mishka
{
    public class JiggleSettings
    {
        public bool EnableJiggle { get; set; }
        public bool ZenMode { get; set; }
        public int IntervalMs { get; set; } = 1000;
        public ModifierKeys HotkeyModifiers { get; set; } = ModifierKeys.Control | ModifierKeys.Shift;
        public Key HotkeyKey { get; set; } = Key.J;
        public bool EnableSchedule { get; set; }
        public DayOfWeek[] ScheduleDays { get; set; } = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        public TimeSpan ScheduleStartTime { get; set; } = new TimeSpan(9, 0, 0);
        public TimeSpan ScheduleEndTime { get; set; } = new TimeSpan(17, 0, 0);
        public bool StartMinimized { get; set; }
        public bool AutoStartup { get; set; }
        public bool StartHidden { get; set; }
    }
    
    public static class SettingsManager
    {
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Mishka",
            "settings.json"
        );
        
        public static JiggleSettings Load()
        {
            return LoadWithValidation();
        }

        public static JiggleSettings LoadWithValidation()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    var settings = JsonSerializer.Deserialize<JiggleSettings>(json);
                    
                    // Validate settings
                    if (settings != null && ValidateSettings(settings))
                    {
                        System.Diagnostics.Debug.WriteLine("Settings loaded and validated successfully");
                        return settings;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Settings validation failed, using defaults");
                        try
                        {
                            WMB.Show(
                                "Settings file is corrupted or contains invalid values.\n\n" +
                                "Using default settings instead.",
                                "Settings Warning",
                                System.Windows.MessageBoxButton.OK,
                                System.Windows.MessageBoxImage.Warning
                            );
                        }
                        catch (Exception msgEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"Cannot show settings warning: {msgEx.Message}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No settings file found, creating default");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Settings load failed: {ex.Message}");
                try
                {
                    WMB.Show(
                        "Settings file could not be read.\n\n" +
                        "Using default settings instead.",
                        "Settings Error",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Warning
                    );
                }
                catch (Exception msgEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Cannot show settings error: {msgEx.Message}");
                }
            }
            
            return new JiggleSettings();
        }

        private static bool ValidateSettings(JiggleSettings settings)
        {
            try
            {
                // Basic validation
                if (settings.IntervalMs <= 0 || settings.IntervalMs > 60000)
                {
                    System.Diagnostics.Debug.WriteLine($"Invalid interval: {settings.IntervalMs}");
                    return false;
                }

                // Validate time spans
                if (settings.ScheduleStartTime.TotalHours < 0 || settings.ScheduleStartTime.TotalHours > 24 ||
                    settings.ScheduleEndTime.TotalHours < 0 || settings.ScheduleEndTime.TotalHours > 24)
                {
                    System.Diagnostics.Debug.WriteLine($"Invalid schedule times");
                    return false;
                }

                // Validate schedule days
                if (settings.ScheduleDays == null || settings.ScheduleDays.Length == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid schedule days array");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Settings validation error: {ex.Message}");
                return false;
            }
        }
        
        public static void Save(JiggleSettings settings)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);
            }
            catch { }
        }
    }
}