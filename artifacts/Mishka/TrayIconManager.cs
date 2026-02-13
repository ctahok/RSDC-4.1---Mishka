using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace Mishka
{
    public class TrayIconManager : IDisposable
    {
        private NotifyIcon? notifyIcon;
        private Window mainWindow;
        
        public event EventHandler? ShowWindowRequested;
        public event EventHandler? ExitRequested;
        public event EventHandler? SettingsRequested;
        
        public TrayIconManager(Window window, string iconText = "Mishka")
        {
            mainWindow = window;
            
            try
            {
                notifyIcon = new NotifyIcon();
                notifyIcon.Text = iconText;
                
                // Create icon from emoji with fallback
                try
                {
                    notifyIcon.Icon = CreateIconFromEmoji("🖱️");
                }
                catch
                {
                    // Fallback: create simple colored icon
                    notifyIcon.Icon = CreateSimpleIcon();
                }
                
                notifyIcon.Visible = true;
                
                // Context menu
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                
                ToolStripMenuItem showItem = new ToolStripMenuItem("Show Mishka");
                showItem.Click += (s, e) => ShowWindowRequested?.Invoke(this, EventArgs.Empty);
                contextMenu.Items.Add(showItem);
                
                ToolStripMenuItem settingsItem = new ToolStripMenuItem("Settings...");
                settingsItem.Click += (s, e) => SettingsRequested?.Invoke(this, EventArgs.Empty);
                contextMenu.Items.Add(settingsItem);
                
                contextMenu.Items.Add(new ToolStripSeparator());
                
                ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
                exitItem.Click += (s, e) => ExitRequested?.Invoke(this, EventArgs.Empty);
                contextMenu.Items.Add(exitItem);
                
                notifyIcon.ContextMenuStrip = contextMenu;
                
                // Double-click to show
                notifyIcon.DoubleClick += (s, e) => ShowWindowRequested?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Tray icon initialization failed: {ex.Message}");
                
                try
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"Failed to create system tray icon: {ex.Message}\n\n" +
                        "The application will continue but you'll need to use the hotkey (Ctrl+Shift+J) to access it.",
                        "Tray Icon Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning
                    );
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine($"Cannot show tray error: {ex.Message}");
                }
                
                notifyIcon?.Dispose();
                notifyIcon = null;
            }
        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        private Icon CreateIconFromEmoji(string emoji)
        {
            // Create a simple colored square icon as fallback
            // For production, use embedded .ico file
            using (Bitmap bmp = new Bitmap(32, 32))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.FromArgb(30, 144, 255)); // DodgerBlue
                    g.DrawString(emoji, new Font("Segoe UI Emoji", 16), Brushes.White, 2, 2);
                }
                IntPtr hIcon = bmp.GetHicon();
                try
                {
                    return (Icon)Icon.FromHandle(hIcon).Clone();
                }
                finally
                {
                    DestroyIcon(hIcon);
                }
            }
        }

        private Icon CreateSimpleIcon()
        {
            // Create a simple colored square icon without emoji
            using (Bitmap bmp = new Bitmap(32, 32))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.FromArgb(30, 144, 255)); // DodgerBlue
                    g.FillEllipse(Brushes.White, 5, 5, 22, 22);
                    g.DrawEllipse(Pens.DodgerBlue, 5, 5, 22, 22);
                }
                IntPtr hIcon = bmp.GetHicon();
                try
                {
                    return (Icon)Icon.FromHandle(hIcon).Clone();
                }
                finally
                {
                    DestroyIcon(hIcon);
                }
            }
        }
        
        public void Dispose()
        {
            notifyIcon?.Dispose();
        }
    }
}
