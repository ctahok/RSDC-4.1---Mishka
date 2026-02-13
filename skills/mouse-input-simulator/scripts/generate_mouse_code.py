import argparse
import os

def generate_csharp_mouse_code(output_path):
    """Generate C# code for mouse simulation."""
    
    csharp_code = '''using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MouseJiggler
{
    public static class MouseSimulator
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);
        
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        
        const uint MOUSEEVENTF_MOVE = 0x0001;
        
        private static Thread? jiggleThread;
        private static bool isRunning = false;
        private static bool zenMode = false;
        
        public static void StartJiggling(bool virtualMode = false, int intervalMs = 1000)
        {
            if (isRunning) return;
            
            zenMode = virtualMode;
            isRunning = true;
            
            jiggleThread = new Thread(() => JiggleLoop(intervalMs));
            jiggleThread.IsBackground = true;
            jiggleThread.Start();
        }
        
        public static void StopJiggling()
        {
            isRunning = false;
            jiggleThread?.Join(100);
        }
        
        public static bool IsJiggling => isRunning;
        public static bool IsZenMode => zenMode;
        
        private static void JiggleLoop(int intervalMs)
        {
            while (isRunning)
            {
                if (zenMode)
                {
                    // Virtual mode: simulate input without moving cursor
                    mouse_event(MOUSEEVENTF_MOVE, 1, 0, 0, 0);
                    Thread.Sleep(10);
                    mouse_event(MOUSEEVENTF_MOVE, 0xFFFFFFFF, 0, 0, 0); // -1 as uint
                }
                else
                {
                    // Physical mode: move cursor slightly
                    GetCursorPos(out POINT point);
                    SetCursorPos(point.X + 1, point.Y);
                    Thread.Sleep(10);
                    SetCursorPos(point.X, point.Y);
                }
                
                Thread.Sleep(intervalMs);
            }
        }
    }
}'''
    
    with open(output_path, 'w') as f:
        f.write(csharp_code)
    
    print(f"✓ Generated C# mouse code: {output_path}")

def generate_powershell_mouse_code(output_path):
    """Generate PowerShell code for mouse simulation."""
    
    ps_code = '''# Mouse Simulator PowerShell Code

Add-Type @"
using System;
using System.Runtime.InteropServices;
using System.Threading;

public class MouseSimulatorPS
{
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int x, int y);
    
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);
    
    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
    
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }
    
    const uint MOUSEEVENTF_MOVE = 0x0001;
    
    private static Thread jiggleThread;
    private static bool isRunning = false;
    private static bool zenMode = false;
    
    public static void StartJiggling(bool virtualMode = false, int intervalMs = 1000)
    {
        if (isRunning) return;
        
        zenMode = virtualMode;
        isRunning = true;
        
        jiggleThread = new Thread(() => JiggleLoop(intervalMs));
        jiggleThread.IsBackground = true;
        jiggleThread.Start();
    }
    
    public static void StopJiggling()
    {
        isRunning = false;
        if (jiggleThread != null)
            jiggleThread.Join(100);
    }
    
    public static bool IsJiggling => isRunning;
    public static bool IsZenMode => zenMode;
    
    private static void JiggleLoop(int intervalMs)
    {
        while (isRunning)
        {
            if (zenMode)
            {
                mouse_event(MOUSEEVENTF_MOVE, 1, 0, 0, 0);
                Thread.Sleep(10);
                mouse_event(MOUSEEVENTF_MOVE, 0xFFFFFFFF, 0, 0, 0);
            }
            else
            {
                POINT point;
                GetCursorPos(out point);
                SetCursorPos(point.X + 1, point.Y);
                Thread.Sleep(10);
                SetCursorPos(point.X, point.Y);
            }
            
            Thread.Sleep(intervalMs);
        }
    }
}
"@
'''
    
    with open(output_path, 'w') as f:
        f.write(ps_code)
    
    print(f"✓ Generated PowerShell mouse code: {output_path}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Generate mouse simulation code")
    parser.add_argument("--language", choices=['csharp', 'powershell'], required=True)
    parser.add_argument("--output", required=True, help="Output file path")
    args = parser.parse_args()
    
    if args.language == 'csharp':
        generate_csharp_mouse_code(args.output)
    else:
        generate_powershell_mouse_code(args.output)