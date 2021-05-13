using System;
using Motion.AdlinkAps;
using Motion.Interfaces;
using System.Collections.Generic;
namespace Motion.Enginee
{
    /// <summary>
    ///     凌华Adlink轴
    /// </summary>
    public class ApsAxis : Axis, INeedClean
    {
        protected readonly ApsController ApsController;

        public ApsAxis(ApsController apsController)
        {
            ApsController = apsController;
        }

        #region Overrides of Axis

        /// <summary>
        ///     当前 Absolute 位置。
        /// </summary>
        public override int CurrentPos
        {
            get{ return ApsController.GetCurrentCommandPosition(NoId); }
        }
        public override int CurrentSpeed
        {
            get
            {
                return ApsController.GetCurrentCommandSpeed(NoId);
            }
        }
        /// <summary>
        ///     是否已励磁。
        /// </summary>
        public bool IsServon
        {
            get { return ApsController.GetServo(NoId); }
            set
            {
                if (value)
                {
                    ApsController.ServoOn(NoId);
                }
                else
                {
                    //ApsController.ServoOff(NoId);
                }
            }
        }

        /// <summary>
        ///     是否已完成最后运动指令。
        /// </summary>
        /// <code>? + var isReach = Math.Abs(commandPosition - currentPosition) &lt; Precision;</code>
        public override bool IsDone
        {
            get { return ApsController.IsDown(NoId); }
        }

        /// <summary>
        /// 运动轴轴移动到指定的位置。
        /// </summary>
        /// <param name="value">将要移动到的位置。</param>
        /// <param name="velocityCurve">移动时的运行参数。</param>
        public override void MoveTo(int value, VelocityCurve velocityCurve = null)
        {
            //ApsController.MoveRelPulse(NoId, value, velocityCurve);
            ApsController.MoveAbsPulse(NoId, value, velocityCurve);
        }

        /// <summary>
        ///     运动轴相对移动到指定位置。
        /// </summary>
        /// <param name="value">要移动到的距离。</param>
        /// <param name="velocityCurve"></param>
        public override void MoveDelta(int value, VelocityCurve velocityCurve = null)
        {
            ApsController.MoveRelPulse(NoId, value, velocityCurve);
        }

        /// <summary>
        ///     正向移动。
        /// </summary>
        public override void Postive()
        {
            var velocityCurve = new VelocityCurve { Maxvel = Speed ?? 0 };
            ApsController.ContinuousMove(NoId, MoveDirection.Postive, velocityCurve);
        }

        /// <summary>
        ///     反向移动。
        /// </summary>
        public override void Negative()
        {
            var velocityCurve = new VelocityCurve { Maxvel = Speed ?? 0 };
            ApsController.ContinuousMove(NoId, MoveDirection.Negative, velocityCurve);
        }

        /// <summary>
        ///     轴停止运动。
        /// </summary>
        /// <param name="velocityCurve"></param>
        public override void Stop(VelocityCurve velocityCurve = null)
        {
            ApsController.DecelStop(NoId);
        }

        public override void Initialize()
        {
            ApsController.MoveOrigin(NoId);
        }

        #endregion

        #region Implementation of INeedInitialization

        #endregion

        #region Implementation of INeedClean

        /// <summary>
        ///      清除
        /// </summary>
        public void Clean()
        {
            Stop();
        }

        public override StopReasons GetStopReasons
        {
            get
            {
                return ApsController.GetStopReason(NoId);
            }
        }
        /// <summary>
        /// 轴报警集合
        /// </summary>
        public IList<Alarm> Alarms
        {
            get
            {
                var list = new List<Alarm>();
                list.Add(new Alarm(() => ApsController.IsAlm(NoId)) { AlarmLevel = AlarmLevels.Error, Name = Name + "故障报警" });
                return list;
            }
        }
        #endregion
    }
}