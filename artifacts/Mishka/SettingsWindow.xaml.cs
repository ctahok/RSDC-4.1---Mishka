using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WMB = System.Windows.MessageBox;
using System.Windows.Interop;

namespace Mishka
{
    public partial class SettingsWindow : Window
    {
        private JiggleSettings settings;
        private bool hasConflict = false;
        
        public SettingsWindow(JiggleSettings currentSettings)
        {
            InitializeComponent();
            settings = currentSettings;
            InitializeUI();
        }
        
        private void InitializeUI()
        {
            // Initialize hotkey dropdowns
            cmbModifiers.Items.Add("Ctrl+Shift");
            cmbModifiers.Items.Add("Ctrl+Alt");
            cmbModifiers.Items.Add("Alt+Shift");
            cmbModifiers.Items.Add("Win");
            cmbModifiers.Items.Add("Ctrl+Win");
            cmbModifiers.Items.Add("Alt+Win");
            cmbModifiers.Items.Add("Shift+Win");
            
            // Select current modifier
            string currentMod = settings.HotkeyModifiers.ToString().Replace(", ", "+");
            int modIndex = cmbModifiers.Items.IndexOf(currentMod);
            cmbModifiers.SelectedIndex = modIndex >= 0 ? modIndex : 0;
            
            // Initialize keys (A-Z, 0-9, F1-F12)
            for (int i = 1; i <= 12; i++)
            {
                cmbKey.Items.Add($"F{i}");
            }
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (key >= Key.A && key <= Key.Z)
                {
                    cmbKey.Items.Add(key.ToString());
                }
            }
            for (int i = 0; i <= 9; i++)
            {
                cmbKey.Items.Add($"D{i}");
            }
            
            cmbKey.SelectedItem = settings.HotkeyKey.ToString();
            
            // Initialize time dropdowns
            for (int i = 0; i < 24; i++)
            {
                cmbStartHour.Items.Add(i.ToString("D2"));
                cmbEndHour.Items.Add(i.ToString("D2"));
            }
            for (int i = 0; i < 60; i += 5)
            {
                cmbStartMinute.Items.Add(i.ToString("D2"));
                cmbEndMinute.Items.Add(i.ToString("D2"));
            }
            
            cmbStartHour.SelectedItem = settings.ScheduleStartTime.Hours.ToString("D2");
            cmbStartMinute.SelectedItem = (settings.ScheduleStartTime.Minutes / 5 * 5).ToString("D2");
            cmbEndHour.SelectedItem = settings.ScheduleEndTime.Hours.ToString("D2");
            cmbEndMinute.SelectedItem = (settings.ScheduleEndTime.Minutes / 5 * 5).ToString("D2");
            
            // Set checkboxes
            chkEnableSchedule.IsChecked = settings.EnableSchedule;
            
            // Set schedule days
            string daysText = "Monday - Friday";
            if (settings.ScheduleDays.Length == 7)
            {
                daysText = "Every Day";
            }
            else if (settings.ScheduleDays.Length == 2 && 
                     settings.ScheduleDays.Contains(DayOfWeek.Saturday) && 
                     settings.ScheduleDays.Contains(DayOfWeek.Sunday))
            {
                daysText = "Weekends Only";
            }
            
            foreach (ComboBoxItem item in cmbScheduleDays.Items)
            {
                if (item.Content.ToString() == daysText)
                {
                    item.IsSelected = true;
                    break;
                }
            }
            
            chkAutoStartup.IsChecked = StartupManager.IsStartupEnabled();
            chkStartHidden.IsChecked = settings.StartHidden;
            chkStartMinimized.IsChecked = settings.StartMinimized;
            
            // Initialize interval
            foreach (ComboBoxItem item in cmbInterval.Items)
            {
                if (item.Content.ToString().StartsWith(settings.IntervalMs.ToString()))
                {
                    item.IsSelected = true;
                    break;
                }
            }
        }
        
        private void BtnTestHotkey_Click(object sender, RoutedEventArgs e)
        {
            var modifiers = GetSelectedModifiers();
            var key = GetSelectedKey();
            
            // Reload original settings to compare with current selection
            var originalSettings = SettingsManager.Load();
            
            var (hasConflict, message) = HotkeyConflictDetector.CheckConflict(modifiers, key, new WindowInteropHelper(this).Handle, originalSettings.HotkeyModifiers, originalSettings.HotkeyKey);
            this.hasConflict = hasConflict;
            
            if (hasConflict)
            {
                txtHotkeyStatus.Text = $"⚠️ {message}";
                txtHotkeyStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
            }
            else
            {
                txtHotkeyStatus.Text = $"✓ {message}";
                txtHotkeyStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGreen);
            }
        }
        
        private ModifierKeys GetSelectedModifiers()
        {
            var modifiers = ModifierKeys.None;
            
            switch (cmbModifiers.SelectedIndex)
            {
                case 0: modifiers = ModifierKeys.Control | ModifierKeys.Shift; break;
                case 1: modifiers = ModifierKeys.Control | ModifierKeys.Alt; break;
                case 2: modifiers = ModifierKeys.Alt | ModifierKeys.Shift; break;
                case 3: modifiers = ModifierKeys.Windows; break;
                case 4: modifiers = ModifierKeys.Control | ModifierKeys.Windows; break;
                case 5: modifiers = ModifierKeys.Alt | ModifierKeys.Windows; break;
                case 6: modifiers = ModifierKeys.Shift | ModifierKeys.Windows; break;
            }
            
            return modifiers;
        }
        
        private Key GetSelectedKey()
        {
            if (cmbKey.SelectedItem != null && Enum.TryParse<Key>(cmbKey.SelectedItem.ToString(), out var key))
            {
                return key;
            }
            return Key.J;
        }
        
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Check for conflict one more time
            var modifiers = GetSelectedModifiers();
            var key = GetSelectedKey();
            
            // Reload original settings to compare
            var originalSettings = SettingsManager.Load();
            
            var (hasConflictNow, message) = HotkeyConflictDetector.CheckConflict(modifiers, key, new WindowInteropHelper(this).Handle, originalSettings.HotkeyModifiers, originalSettings.HotkeyKey);
            
            if (hasConflictNow)
            {
                System.Windows.MessageBox.Show(
                    message,
                    "Hotkey Conflict - Cannot Save",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
            // Save settings
            settings.HotkeyModifiers = modifiers;
            settings.HotkeyKey = key;
            settings.EnableSchedule = chkEnableSchedule.IsChecked ?? false;
            settings.StartHidden = chkStartHidden.IsChecked ?? false;
            settings.StartMinimized = chkStartMinimized.IsChecked ?? false;
            
            // Schedule days
            string selectedDays = ((ComboBoxItem)cmbScheduleDays.SelectedItem)?.Content.ToString() ?? "Monday - Friday";
            switch (selectedDays)
            {
                case "Every Day":
                    settings.ScheduleDays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToArray();
                    break;
                case "Weekends Only":
                    settings.ScheduleDays = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
                    break;
                default: // Monday - Friday
                    settings.ScheduleDays = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
                    break;
            }
            
            // Schedule times
            if (int.TryParse(cmbStartHour.SelectedItem?.ToString(), out int startHour) &&
                int.TryParse(cmbStartMinute.SelectedItem?.ToString(), out int startMinute))
            {
                settings.ScheduleStartTime = new TimeSpan(startHour, startMinute, 0);
            }
            
            if (int.TryParse(cmbEndHour.SelectedItem?.ToString(), out int endHour) &&
                int.TryParse(cmbEndMinute.SelectedItem?.ToString(), out int endMinute))
            {
                settings.ScheduleEndTime = new TimeSpan(endHour, endMinute, 0);
            }
            
            // Interval
            string intervalText = ((ComboBoxItem)cmbInterval.SelectedItem)?.Content.ToString() ?? "1000 ms";
            if (int.TryParse(intervalText.Split(' ')[0], out int intervalMs))
            {
                settings.IntervalMs = intervalMs;
            }
            
            // Auto-startup
            bool autoStartup = chkAutoStartup.IsChecked ?? false;
            if (autoStartup)
            {
                StartupManager.EnableStartup();
            }
            else
            {
                StartupManager.DisableStartup();
            }
            settings.AutoStartup = autoStartup;
            
            SettingsManager.Save(settings);
            
            DialogResult = true;
            Close();
        }
        
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}