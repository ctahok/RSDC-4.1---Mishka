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
        
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        
        const uint MOUSEEVENTF_MOVE = 0x0001;
        
        private static Thread? jiggleThread;
        private static volatile bool isRunning = false;
        private static volatile bool zenMode = false;
        private static volatile int intervalMs = 1000;
        
        public static event EventHandler? StatusChanged;
        
        public static void Start(bool virtualMode = false, int interval = 1000)
        {
            if (isRunning) return;
            
            zenMode = virtualMode;
            intervalMs = interval;
            isRunning = true;
            
            jiggleThread = new Thread(JiggleLoop);
            jiggleThread.IsBackground = true;
            jiggleThread.Start();
            
            StatusChanged?.Invoke(null, EventArgs.Empty);
        }
        
        public static void Stop()
        {
            isRunning = false;
            jiggleThread?.Join(100);
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
                        mouse_event(MOUSEEVENTF_MOVE, 1, 0, 0, 0);
                        Thread.Sleep(10);
                        mouse_event(MOUSEEVENTF_MOVE, 0xFFFFFFFF, 0, 0, 0);
                    }
                    else
                    {
                        GetCursorPos(out POINT point);
                        SetCursorPos(point.X + 1, point.Y);
                        Thread.Sleep(10);
                        SetCursorPos(point.X, point.Y);
                    }
                }
                catch { }
                
                Thread.Sleep(intervalMs);
            }
        }
    }
}
