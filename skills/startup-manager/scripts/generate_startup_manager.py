#!/usr/bin/env python3
"""
Generate C# startup manager class for Windows auto-startup.
"""
import argparse
import os

def generate_code(app_name):
    return f'''using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace Mishka
{{
    public static class StartupManager
    {{
        private const string RUN_KEY = @"Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string APP_NAME = "{app_name}";
        
        public static bool IsStartupEnabled()
        {{
            try
            {{
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_KEY, false))
                {{
                    if (key != null)
                    {{
                        string value = key.GetValue(APP_NAME) as string;
                        return !string.IsNullOrEmpty(value);
                    }}
                }}
            }}
            catch (Exception ex)
            {{
                System.Diagnostics.Debug.WriteLine($"Error checking startup: {{ex.Message}}");
            }}
            return false;
        }}
        
        public static void EnableStartup()
        {{
            try
            {{
                string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                // For single-file publish, get the .exe path
                if (executablePath.EndsWith(".dll"))
                {{
                    executablePath = executablePath.Replace(".dll", ".exe");
                }}
                
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_KEY, true))
                {{
                    key?.SetValue(APP_NAME, executablePath);
                }}
            }}
            catch (Exception ex)
            {{
                MessageBox.Show($"Failed to enable startup: {{ex.Message}}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }}
        }}
        
        public static void DisableStartup()
        {{
            try
            {{
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_KEY, true))
                {{
                    key?.DeleteValue(APP_NAME, false);
                }}
            }}
            catch (Exception ex)
            {{
                MessageBox.Show($"Failed to disable startup: {{ex.Message}}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }}
        }}
    }}
}}'''

def main():
    parser = argparse.ArgumentParser(description='Generate startup manager')
    parser.add_argument('--output', required=True, help='Output file path')
    parser.add_argument('--app-name', default='Mishka', help='Application name for registry')
    args = parser.parse_args()
    
    code = generate_code(args.app_name)
    
    with open(args.output, 'w') as f:
        f.write(code)
    
    print(f"✓ Generated startup manager: {args.output}")

if __name__ == '__main__':
    main()