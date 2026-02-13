using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Mishka
{
    public static class HotkeyConflictDetector
    {
        [DllImport("user32.dll")]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        
        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;
        private const int TEST_HOTKEY_ID = 99999;
        
        public static (bool hasConflict, string message) CheckConflict(ModifierKeys modifiers, Key key, IntPtr windowHandle, ModifierKeys? currentModifiers = null, Key? currentKey = null)
        {
            if (windowHandle == IntPtr.Zero)
            {
                return (true, "No valid window handle available for hotkey testing.");
            }

            // If it's the same as current, it's not a conflict for THIS app
            if (currentModifiers.HasValue && currentKey.HasValue && 
                modifiers == currentModifiers.Value && key == currentKey.Value)
            {
                return (false, $"Hotkey {GetModifierString(modifiers)}+{key} is currently in use by Mishka.");
            }
            
            try
            {
                // Convert modifiers to Windows API flags
                uint modFlags = 0;
                if (modifiers.HasFlag(ModifierKeys.Alt)) modFlags |= MOD_ALT;
                if (modifiers.HasFlag(ModifierKeys.Control)) modFlags |= MOD_CONTROL;
                if (modifiers.HasFlag(ModifierKeys.Shift)) modFlags |= MOD_SHIFT;
                if (modifiers.HasFlag(ModifierKeys.Windows)) modFlags |= MOD_WIN;
                
                uint vk = (uint)KeyInterop.VirtualKeyFromKey(key);
                
                // Attempt test registration with real window handle
                bool canRegister = RegisterHotKey(windowHandle, TEST_HOTKEY_ID, modFlags, vk);
                
                if (!canRegister)
                {
                    string modStr = GetModifierString(modifiers);
                    return (true, $"Hotkey {modStr}+{key} is already in use by another application.\n\nPlease choose a different combination.");
                }
                
                // Clean up test registration
                UnregisterHotKey(windowHandle, TEST_HOTKEY_ID);
                
                return (false, $"Hotkey {GetModifierString(modifiers)}+{key} is available.");
            }
            catch (Exception ex)
            {
                return (true, $"Error checking hotkey: {ex.Message}");
            }
        }
        
        private static string GetModifierString(ModifierKeys modifiers)
        {
            var parts = new System.Collections.Generic.List<string>();
            if (modifiers.HasFlag(ModifierKeys.Control)) parts.Add("Ctrl");
            if (modifiers.HasFlag(ModifierKeys.Alt)) parts.Add("Alt");
            if (modifiers.HasFlag(ModifierKeys.Shift)) parts.Add("Shift");
            if (modifiers.HasFlag(ModifierKeys.Windows)) parts.Add("Win");
            return string.Join("+", parts);
        }
    }
}