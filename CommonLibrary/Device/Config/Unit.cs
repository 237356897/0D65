using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CommonLibrary
{
    public class Unit
    {
        public static Common.void_StringDelegate Alarm;
        public Func<bool> IsAction;  

        private int alarmCount = -1;
        private Func<int> alarmDelay;
        private Func<string> alarmMsg;
        private Func<bool> isAlarm;
        private static List<Unit> units = new List<Unit>();

        /// <summary>
        /// 实例化机构单元体
        /// </summary>
        /// <param name="actionCondition">Bool方法：动作条件</param>
        /// <param name="alarmCondition">Bool方法：报警条件</param>
        /// <param name="alarmdelay">int方法：报警延时</param>
        /// <param name="alarmmsg">string方法：报警信息</param>
        public Unit(Func<bool> actionCondition,Func<bool> alarmCondition,Func<int> alarmDelay,Func<string> alarmMsg)
        {
            IsAction = actionCondition;
            isAlarm = alarmCondition;
            this.alarmDelay = alarmDelay;
            this.alarmMsg = alarmMsg;
            units.Add(this);
        }

        #region Main Call
        public static void Run()
        {
            Thread runningThread = new Thread(new ThreadStart(running));
            runningThread.IsBackground = true;
            runningThread.Start();
        }

        private static void running()
        {
            while (true)
            {
                for (int i = 0; i < units.Count; ++i)
                {
                    if (units[i].isAlarm() && units[i].alarmCount != -100)
                    {
                        ++units[i].alarmCount;
                    }
                    else if (!units[i].isAlarm())
                    {
                        units[i].alarmCount = -1;
                    }
                    if (units[i].alarmCount >= (units[i].alarmDelay() / 20))
                    {
                        Alarm.Invoke(units[i].alarmMsg());
                        units[i].alarmCount = -100;
                    }
                }

                Thread.Sleep(20);
            }
        }

        #endregion
    }
}
