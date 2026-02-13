using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace Mishka
{
    public class HotkeyManager : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;
        
        private Window window;
        private IntPtr windowHandle;
        private int hotkeyId = 9000;
        private bool isRegistered = false;
        
        // Pending registration
        private bool hasPendingRegistration = false;
        private ModifierKeys pendingModifiers;
        private Key pendingKey;
        
        public event EventHandler? HotkeyPressed;
        
        public HotkeyManager(Window window)
        {
            this.window = window;
            
            if (window.IsLoaded)
            {
                Initialize();
            }
            else
            {
                window.Loaded += (s, e) => Initialize();
            }
        }
        
        private void Initialize()
        {
            windowHandle = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            
            // Add hook for hotkey messages
            System.Windows.Interop.HwndSource source = System.Windows.Interop.HwndSource.FromHwnd(windowHandle);
            source?.AddHook(WndProc);
            
            // Process pending registration
            if (hasPendingRegistration)
            {
                RegisterHotKey(pendingModifiers, pendingKey);
                hasPendingRegistration = false;
            }
        }
        
        public bool RegisterHotKey(ModifierKeys modifiers, Key key)
        {
            // If window not loaded, queue it
            if (windowHandle == IntPtr.Zero)
            {
                pendingModifiers = modifiers;
                pendingKey = key;
                hasPendingRegistration = true;
                return true;
            }
            
            if (isRegistered)
            {
                UnregisterHotKey(windowHandle, hotkeyId);
                isRegistered = false;
            }
            
            uint modFlags = 0;
            if (modifiers.HasFlag(ModifierKeys.Alt)) modFlags |= MOD_ALT;
            if (modifiers.HasFlag(ModifierKeys.Control)) modFlags |= MOD_CONTROL;
            if (modifiers.HasFlag(ModifierKeys.Shift)) modFlags |= MOD_SHIFT;
            if (modifiers.HasFlag(ModifierKeys.Windows)) modFlags |= MOD_WIN;
            
            uint vk = (uint)KeyInterop.VirtualKeyFromKey(key);
            
            isRegistered = RegisterHotKey(windowHandle, hotkeyId, modFlags, vk);
            return isRegistered;
        }
        
        public void UnregisterHotKey()
        {
            if (isRegistered && windowHandle != IntPtr.Zero)
            {
                UnregisterHotKey(windowHandle, hotkeyId);
                isRegistered = false;
            }
        }
        
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            
            if (msg == WM_HOTKEY && wParam.ToInt32() == hotkeyId)
            {
                HotkeyPressed?.Invoke(this, EventArgs.Empty);
                handled = true;
            }
            
            return IntPtr.Zero;
        }
        
        public void Dispose()
        {
            UnregisterHotKey();
        }
    }
}