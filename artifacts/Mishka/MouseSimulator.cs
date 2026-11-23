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
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(out LASTINPUTINFO plii);

        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(uint esFlags);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
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
        private static volatile bool stopOnUserActivity = true;
        private static volatile int userInactivityTimeoutSeconds = 30;
        private static volatile bool isPausedByUserActivity = false;
        private static volatile bool userActivityMonitorRunning = false;
        private static Thread? userActivityMonitorThread;
        private static int jiggleOffset = 0;
        
        public static event EventHandler? StatusChanged;
        
        public static void Start(bool virtualMode = false, int interval = 1000, bool stopOnUserActivity = true, int userInactivityTimeoutSeconds = 30)
        {
            if (isRunning) return;

            zenMode = virtualMode;
            intervalMs = interval;
            MouseSimulator.stopOnUserActivity = stopOnUserActivity;
            MouseSimulator.userInactivityTimeoutSeconds = userInactivityTimeoutSeconds;
            isRunning = true;
            isPausedByUserActivity = false;
            jiggleOffset = 0;

            // Prevent system sleep and screen saver activation
            SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);
            jiggleThread = new Thread(JiggleLoop);
            jiggleThread.IsBackground = true;
            jiggleThread.Start();

            StartUserActivityMonitor();

            StatusChanged?.Invoke(null, EventArgs.Empty);
        }
        
        public static void Stop()
        {
            isRunning = false;
            isPausedByUserActivity = false;
            StopUserActivityMonitor();
            jiggleThread?.Join(100);
            
            // Restore normal power management
            SetThreadExecutionState(ES_CONTINUOUS);
            
            StatusChanged?.Invoke(null, EventArgs.Empty);
        }
        
        public static void Pause()
        {
            isPausedByUserActivity = true;
            StatusChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void Resume()
        {
            isPausedByUserActivity = false;
            StatusChanged?.Invoke(null, EventArgs.Empty);
        }

        private static void StartUserActivityMonitor()
        {
            if (userActivityMonitorRunning) return;
            userActivityMonitorRunning = true;

            userActivityMonitorThread = new Thread(UserActivityMonitorLoop);
            userActivityMonitorThread.IsBackground = true;
            userActivityMonitorThread.Start();
        }

        private static void StopUserActivityMonitor()
        {
            userActivityMonitorRunning = false;
            userActivityMonitorThread?.Join(200);
            userActivityMonitorThread = null;
        }

        private static void UserActivityMonitorLoop()
        {
            while (userActivityMonitorRunning)
            {
                try
                {
                    if (GetLastInputInfo(out LASTINPUTINFO lastInputInfo))
                    {
                        uint currentTicks = (uint)Environment.TickCount;
                        uint idleTicks = currentTicks - lastInputInfo.dwTime;
                        int idleSeconds = (int)(idleTicks / 1000);

                        if (idleSeconds < 2)
                        {
                            if (isRunning && !isPausedByUserActivity && stopOnUserActivity)
                            {
                                Pause();
                            }
                        }
                        else if (isPausedByUserActivity && idleSeconds >= userInactivityTimeoutSeconds)
                        {
                            Resume();
                        }
                    }
                }
                catch { }

                Thread.Sleep(500);
            }
        }

        public static bool IsRunning => isRunning;
        public static bool IsZenMode => zenMode;
        public static int Interval => intervalMs;
        public static bool IsPausedByUserActivity => isPausedByUserActivity;
        public static bool StopOnUserActivity => stopOnUserActivity;
        public static int UserInactivityTimeoutSeconds => userInactivityTimeoutSeconds;
        
        private static void JiggleLoop()
        {
            while (isRunning)
            {
                try
                {
                    if (zenMode)
                    {
                        // Zen mode: Minimal cursor movement (barely noticeable) to prevent screensaver
                        // Refresh the execution state to keep system awake
                        SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);

                        GetCursorPos(out POINT point);
                        SetCursorPos(point.X + 1, point.Y);
                        Thread.Sleep(10);
                        SetCursorPos(point.X, point.Y);
                    }
                    else
                    {
                        // Physical mode: Actually move the cursor for visual feedback
                        GetCursorPos(out POINT point);
                        // Alternate between moving right and left for visible jiggle
                        jiggleOffset = (jiggleOffset == 0) ? 5 : 0;
                        SetCursorPos(point.X + jiggleOffset, point.Y);

                        Thread.Sleep(50); // Brief pause so movement is visible

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
