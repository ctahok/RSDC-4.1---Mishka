using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using WMB = System.Windows.MessageBox;

namespace Mishka
{
    public partial class MainWindow : Window
    {
        private JiggleSettings settings;
        private TrayIconManager? trayIconManager;
        private HotkeyManager? hotkeyManager;
        private ScheduleManager? scheduleManager;
        private System.Windows.Threading.DispatcherTimer? statusTimer;
        
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                
                // Set window icon from emoji
                try {
                    this.Icon = CreateImageSourceFromEmoji("🖱️");
                } catch { }
                
                // Enhanced settings loading with validation
                settings = SettingsManager.LoadWithValidation();
                
                InitializeUI();
                
                // Safe tray icon initialization
                bool trayIconSuccess = false;
                try
                {
                    InitializeTrayIcon();
                    trayIconSuccess = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Tray icon init failed: {ex.Message}");
                    // Continue without tray icon - we already showed error message in TrayIconManager
                }
                
                InitializeHotkey();
                InitializeSchedule();
                
                // Timer setup
                statusTimer = new System.Windows.Threading.DispatcherTimer 
                { 
                    Interval = TimeSpan.FromSeconds(1) 
                };
                statusTimer.Tick += StatusTimer_Tick;
                statusTimer.Start();
                
                // Enhanced visibility logic with fallback
                ApplyVisibilitySettings(trayIconSuccess);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MainWindow initialization failed: {ex.Message}");
                
                try
                {
                    WMB.Show(
                        $"Failed to initialize main window: {ex.Message}",
                        "Initialization Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
                catch (Exception msgEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Cannot show initialization error: {msgEx.Message}");
                }
            }
        }

        private void ApplyVisibilitySettings(bool trayIconSuccess)
        {
            try
            {
                if (settings.StartMinimized)
                {
                    Hide();
                    WindowState = WindowState.Minimized;
                }
                else if (settings.StartHidden)
                {
                    Hide();
                }
                else
                {
                    // Default: ensure window is visible
                    Show();
                    WindowState = WindowState.Normal;
                    
                    // Make sure window is actually visible
                    if (!IsVisible)
                    {
                        Show();
                        Activate();
                    }
                }
                
                // If tray icon failed and we're supposed to be hidden/minimized,
                // show the window anyway so user can access the app
                if (!trayIconSuccess && (settings.StartHidden || settings.StartMinimized))
                {
                    System.Diagnostics.Debug.WriteLine("Tray icon failed, forcing window visibility");
                    Show();
                    WindowState = WindowState.Normal;
                    Activate();
                    
                    try
                    {
                         WMB.Show(
                            $"The selected hotkey is already in use by another application.\n\n" +
                            "Please choose a different combination.",
                            "Hotkey Conflict",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                    }
                    catch (Exception msgEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Cannot show tray icon fallback message: {msgEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Visibility settings failed: {ex.Message}");
                // Fallback to visible window
                Show();
                WindowState = WindowState.Normal;
                Activate();
            }
        }
        
        private void InitializeUI()
        {
            // Set ZenMode first so it's ready when EnableJiggle fires events
            chkZenMode.IsChecked = settings.ZenMode;
            
            // Enable/Disable ZenMode UI state based on settings
            chkZenMode.IsEnabled = settings.EnableJiggle;

            // Set EnableJiggle last to trigger startup jiggling if enabled
            chkEnableJiggle.IsChecked = settings.EnableJiggle;
            
            UpdateStatus();
        }
        
        private void InitializeTrayIcon()
        {
            trayIconManager = new TrayIconManager(this);
            trayIconManager.ShowWindowRequested += (s, e) => ShowMainWindow();
            trayIconManager.SettingsRequested += (s, e) => ShowSettings();
            trayIconManager.ExitRequested += (s, e) => ExitApplication();
        }
        
        private void InitializeHotkey()
        {
            hotkeyManager = new HotkeyManager(this);
            hotkeyManager.HotkeyPressed += (s, e) => ToggleWindowVisibility();
            hotkeyManager.RegisterHotKey(settings.HotkeyModifiers, settings.HotkeyKey);
        }
        
        private void InitializeSchedule()
        {
            scheduleManager = new ScheduleManager(settings);
            scheduleManager.ScheduleTriggered += (s, e) => 
            {
                Dispatcher.Invoke(() =>
                {
                    chkEnableJiggle.IsChecked = true;
                    StartJiggling();
                });
            };
            scheduleManager.ScheduleEnded += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    chkEnableJiggle.IsChecked = false;
                    StopJiggling();
                });
            };
            
            if (settings.EnableSchedule)
            {
                scheduleManager.Start();
            }
        }
        
        private void ChkEnableJiggle_Checked(object sender, RoutedEventArgs e)
        {
            chkZenMode.IsEnabled = true;
            settings.EnableJiggle = true;
            StartJiggling();
        }
        
        private void ChkEnableJiggle_Unchecked(object sender, RoutedEventArgs e)
        {
            chkZenMode.IsEnabled = false;
            settings.EnableJiggle = false;
            StopJiggling();
        }

        private void ChkZenMode_Checked(object sender, RoutedEventArgs e)
        {
            settings.ZenMode = true;
            SettingsManager.Save(settings);
            if (settings.EnableJiggle)
            {
                StartJiggling();
            }
        }

        private void ChkZenMode_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.ZenMode = false;
            SettingsManager.Save(settings);
            if (settings.EnableJiggle)
            {
                StartJiggling();
            }
        }
        
        private void StartJiggling()
        {
            MouseSimulator.Start(chkZenMode.IsChecked ?? false, settings.IntervalMs);
            UpdateStatus();
            SettingsManager.Save(settings);
        }
        
        private void StopJiggling()
        {
            MouseSimulator.Stop();
            UpdateStatus();
            SettingsManager.Save(settings);
        }
        
        private void StatusTimer_Tick(object? sender, EventArgs e)
        {
            UpdateStatus();
        }
        
        private void UpdateStatus()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(UpdateStatus);
                return;
            }
            if (MouseSimulator.IsRunning)
            {
                string mode = MouseSimulator.IsZenMode ? " (Zen Mode)" : "";
                txtStatus.Text = $"Status: Running{mode}";
                txtStatus.Foreground = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                txtStatus.Text = "Status: Stopped";
                txtStatus.Foreground = new SolidColorBrush(Colors.Gray);
            }
            
            string modStr = settings.HotkeyModifiers.ToString().Replace(", ", "+");
            txtHotkey.Text = $"Press {modStr}+{settings.HotkeyKey} to hide/show";
        }
        
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            ShowSettings();
        }
        
        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            if (this.IsVisible)
            {
                aboutWindow.Owner = this;
            }
            aboutWindow.ShowDialog();
        }
        
        private void ShowSettings()
        {
            var settingsWindow = new SettingsWindow(settings);
            if (this.IsVisible)
            {
                settingsWindow.Owner = this;
            }
            
            if (settingsWindow.ShowDialog() == true)
            {
                // Reload settings
                settings = SettingsManager.Load();
                
                // Update hotkey
                hotkeyManager?.UnregisterHotKey();
                hotkeyManager?.RegisterHotKey(settings.HotkeyModifiers, settings.HotkeyKey);
                
                // Update schedule
                scheduleManager?.UpdateSettings(settings);
                
                // Update UI
                InitializeUI();
            }
        }
        
        private ImageSource CreateImageSourceFromEmoji(string emoji)
        {
            using (var bmp = new Bitmap(64, 64))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(System.Drawing.Color.Transparent);
                    g.DrawString(emoji, new Font("Segoe UI Emoji", 40), System.Drawing.Brushes.White, 0, 0);
                }
                var hBitmap = bmp.GetHbitmap();
                try
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(hBitmap);
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private void Window_StateChanged(object? sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }
        
        private void ShowMainWindow()
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = WindowState.Normal;
            this.Activate();
            this.Focus();
        }
        
        private void ToggleWindowVisibility()
        {
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                ShowMainWindow();
            }
        }
        
        private void ExitApplication()
        {
            Close();
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MouseSimulator.Stop();
            hotkeyManager?.Dispose();
            scheduleManager?.Stop();
            statusTimer?.Stop();
            trayIconManager?.Dispose();
            
            SettingsManager.Save(settings);
        }
    }
}