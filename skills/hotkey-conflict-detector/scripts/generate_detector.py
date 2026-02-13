#!/usr/bin/env python3
"""
Generate C# hotkey conflict detection class.
"""
import argparse
import os

CSHARP_CODE = '''using System;
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
        
        public static (bool hasConflict, string message) CheckConflict(ModifierKeys modifiers, Key key)
        {
            // Create temporary window handle for testing
            IntPtr testHwnd = IntPtr.Zero;
            
            try
            {
                // Convert modifiers to Windows API flags
                uint modFlags = 0;
                if (modifiers.HasFlag(ModifierKeys.Alt)) modFlags |= MOD_ALT;
                if (modifiers.HasFlag(ModifierKeys.Control)) modFlags |= MOD_CONTROL;
                if (modifiers.HasFlag(ModifierKeys.Shift)) modFlags |= MOD_SHIFT;
                if (modifiers.HasFlag(ModifierKeys.Windows)) modFlags |= MOD_WIN;
                
                uint vk = (uint)KeyInterop.VirtualKeyFromKey(key);
                
                // Attempt test registration
                bool canRegister = RegisterHotKey(testHwnd, TEST_HOTKEY_ID, modFlags, vk);
                
                if (!canRegister)
                {
                    string modStr = modifiers.ToString().Replace(", ", "+");
                    return (true, $"Hotkey {modStr}+{key} is already in use by another application.");
                }
                
                // Clean up test registration
                UnregisterHotKey(testHwnd, TEST_HOTKEY_ID);
                
                return (false, "Hotkey is available.");
            }
            catch (Exception ex)
            {
                return (true, $"Error checking hotkey: {ex.Message}");
            }
        }
    }
}'''

def main():
    parser = argparse.ArgumentParser(description='Generate hotkey conflict detector')
    parser.add_argument('--output', required=True, help='Output file path')
    args = parser.parse_args()
    
    with open(args.output, 'w') as f:
        f.write(CSHARP_CODE)
    
    print(f"✓ Generated conflict detector: {args.output}")

if __name__ == '__main__':
    main()