using System;
using System.Linq;
using System.Threading;
using System.Windows.Threading;

namespace Mishka
{
    public class ScheduleManager
    {
        private System.Threading.Timer? scheduleTimer;
        private JiggleSettings settings;
        private bool isInScheduledTime = false;
        
        public event EventHandler? ScheduleTriggered;
        public event EventHandler? ScheduleEnded;
        
        public ScheduleManager(JiggleSettings settings)
        {
            this.settings = settings;
        }
        
        public void Start()
        {
            if (!settings.EnableSchedule) return;
            
            scheduleTimer = new System.Threading.Timer(CheckSchedule, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }
        
        public void Stop()
        {
            scheduleTimer?.Dispose();
            scheduleTimer = null;
        }
        
        public void UpdateSettings(JiggleSettings newSettings)
        {
            settings = newSettings;
            
            if (settings.EnableSchedule && scheduleTimer == null)
            {
                Start();
            }
            else if (!settings.EnableSchedule && scheduleTimer != null)
            {
                Stop();
            }
        }
        
        private void CheckSchedule(object? state)
        {
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            var isScheduledDay = settings.ScheduleDays.Contains(now.DayOfWeek);
            var isInTimeWindow = currentTime >= settings.ScheduleStartTime && currentTime <= settings.ScheduleEndTime;
            var shouldBeActive = isScheduledDay && isInTimeWindow;
            
            if (shouldBeActive && !isInScheduledTime)
            {
                isInScheduledTime = true;
                ScheduleTriggered?.Invoke(this, EventArgs.Empty);
            }
            else if (!shouldBeActive && isInScheduledTime)
            {
                isInScheduledTime = false;
                ScheduleEnded?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public bool IsInScheduledTime => isInScheduledTime;
    }
}