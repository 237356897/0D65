using System;
using System.Collections.Generic;
using System.Threading;
using Motion.Interfaces;

namespace Motion.Enginee
{
    public class ModelAlarms : ThreadPart, IAlarmsResult
    {
        public List<Alarm> Alarms { get; set; }
        public bool IsAlarms { get; set; }
        public bool IsPrompt { get; set; }
        public bool IsWarning { get; set; }
        public ModelAlarms()
        {
            Alarms = new List<Alarm>();
        }
        public void Add(IList<Alarm> alarms)
        {
            if (alarms != null)
            {
                foreach (Alarm alarm in alarms)
                {
                    Alarms.Add(alarm);
                }
            }
        }
        public override void Running(RunningModes runningMode)
        {
            while (true)
            {
                Thread.Sleep(10);
                var _alarmsStatus = false;
                var _promptStatus = false;
                var _warningStatus = false;
                if (runningMode != RunningModes.None)
                {
                    foreach (Alarm alarm in Alarms)
                    {
                        var btemp = alarm.IsAlarm;
                        if (alarm.AlarmLevel == AlarmLevels.Error)
                        {
                            _alarmsStatus |= btemp;
                        }
                        else if(alarm.AlarmLevel == AlarmLevels.None)
                        {
                            _promptStatus |= btemp;
                        }
                        else
                        {
                            _warningStatus |= btemp;
                        }
                    }
                }
                else
                {
                    break;
                }
                IsAlarms = _alarmsStatus;
                IsPrompt = _promptStatus;
                IsWarning = _warningStatus;
            }
        }
    }
}
