#!/usr/bin/env python3
"""
Generate C# system tray manager class.
"""
import argparse
import os

CSHARP_CODE = '''using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace Mishka
{
    public class TrayIconManager : IDisposable
    {
        private NotifyIcon notifyIcon;
        private Window mainWindow;
        
        public event EventHandler ShowWindowRequested;
        public event EventHandler ExitRequested;
        public event EventHandler SettingsRequested;
        
        public TrayIconManager(Window window, string iconText = "Mishka")
        {
            mainWindow = window;
            
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = iconText;
            notifyIcon.Visible = true;
            
            // Create icon from emoji
            notifyIcon.Icon = CreateIconFromEmoji("🖱️");
            
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
        
        private Icon CreateIconFromEmoji(string emoji)
        {
            // Create a simple colored square icon as fallback
            // For production, use embedded .ico file
            Bitmap bmp = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(30, 144, 255)); // DodgerBlue
                g.DrawString(emoji, new Font("Segoe UI Emoji", 16), Brushes.White, 2, 2);
            }
            return Icon.FromHandle(bmp.GetHicon());
        }
        
        public void ShowNotification(string title, string message, ToolTipIcon icon = ToolTipIcon.Info)
        {
            notifyIcon.ShowBalloonTip(3000, title, message, icon);
        }
        
        public void Dispose()
        {
            notifyIcon?.Dispose();
        }
    }
}'''

def main():
    parser = argparse.ArgumentParser(description='Generate tray manager')
    parser.add_argument('--output', required=True, help='Output file path')
    parser.add_argument('--icon', default='🖱️', help='Icon emoji')
    args = parser.parse_args()
    
    # Replace emoji in template
    code = CSHARP_CODE.replace('🖱️', args.icon)
    
    with open(args.output, 'w') as f:
        f.write(code)
    
    print(f"✓ Generated tray manager: {args.output}")

if __name__ == '__main__':
    main()