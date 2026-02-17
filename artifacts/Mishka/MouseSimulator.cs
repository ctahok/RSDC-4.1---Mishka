using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Mishka
{
    public static class MouseSimulator
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);
        
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        
        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(uint esFlags);
        
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        
        const uint MOUSEEVENTF_MOVE = 0x0001;
        
        // Execution State flags to prevent screen saver and sleep
        const uint ES_CONTINUOUS = 0x80000000;
        const uint ES_DISPLAY_REQUIRED = 0x00000002;
        const uint ES_SYSTEM_REQUIRED = 0x00000001;
        
        private static Thread? jiggleThread;
        private static volatile bool isRunning = false;
        private static volatile bool zenMode = false;
        private static volatile int intervalMs = 1000;
        private static int jiggleOffset = 0;
        
        public static event EventHandler? StatusChanged;
        
        public static void Start(bool virtualMode = false, int interval = 1000)
        {
            if (isRunning) return;
            
            zenMode = virtualMode;
            intervalMs = interval;
            isRunning = true;
            jiggleOffset = 0;
            
            // Prevent system sleep and screen saver activation
            SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);
            
            jiggleThread = new Thread(JiggleLoop);
            jiggleThread.IsBackground = true;
            jiggleThread.Start();
            
            StatusChanged?.Invoke(null, EventArgs.Empty);
        }
        
        public static void Stop()
        {
            isRunning = false;
            jiggleThread?.Join(100);
            
            // Restore normal power management
            SetThreadExecutionState(ES_CONTINUOUS);
            
            StatusChanged?.Invoke(null, EventArgs.Empty);
        }
        
        public static bool IsRunning => isRunning;
        public static bool IsZenMode => zenMode;
        public static int Interval => intervalMs;
        
        private static void JiggleLoop()
        {
            while (isRunning)
            {
                try
                {
                    if (zenMode)
                    {
                        // Zen mode: Use execution state only (no visible movement)
                        // Refresh the execution state to keep system awake
                        SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);
                    }
                    else
                    {
                        // Physical mode: Minimal cursor movement (barely noticeable)
                        GetCursorPos(out POINT point);
                        
                        // Tiny 1-pixel movement
                        jiggleOffset = (jiggleOffset == 0) ? 1 : 0;
                        SetCursorPos(point.X + jiggleOffset, point.Y);
                        
                        Thread.Sleep(10); // Very brief pause
                        
                        // Move back to original position
                        if (jiggleOffset != 0)
                        {
                            SetCursorPos(point.X, point.Y);
                        }
                    }
                }
                catch { }
                
                Thread.Sleep(intervalMs);
            }
        }
    }
}
