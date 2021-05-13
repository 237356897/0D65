using System;
using System.Collections.Generic;
using System.Diagnostics;
using Motion.Interfaces;
namespace Motion.Enginee
{
    public class DoubleCylinder : Cylinder
    {
        #region "字段"
        private bool _noMoveAlarm;//无动点信号报警
        private bool _noOriginAlarm;//无原点信号报警
        private bool _offMoveAlarm;//气缸为Off时，动点信号报警
        private bool _onOriginAlarm;//气缸为On时，原点信号报警
        private bool cylinderNoOrigin;
        private bool cylinderNoMove;
        private bool cylinderOffMove;
        private bool cylinderOnOrigin;
        private Sensor _InOrigin;
        private Sensor _InMove;
        private Signal _OutOrigin;
        private Signal _OutMove;
        private bool cylinderManual = false;
        private bool cylinderEnable = false;
        private bool _autoExecute;
        private readonly Stopwatch _watchOrigin = new Stopwatch();
        private readonly Stopwatch _watchMove = new Stopwatch();
        private readonly Stopwatch[] _watchAlarm =new Stopwatch[] {new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        #endregion
        public DoubleCylinder(Sensor InOrigin, Sensor InMove, Signal OutOrigin, Signal OutMove)
        {
            _InOrigin = InOrigin;
            _InMove = InMove;
            _OutOrigin = OutOrigin;
            _OutMove = OutMove;
            _watchOrigin.Restart();
            _watchMove.Restart();
        }
        public DoubleCylinder(CylinderCondition cylinderCondition,CylinderDelay cylinderDelay,Sensor InOrigin,Sensor InMove,Signal OutOrigin,Signal OutMove)
            :this(InOrigin, InMove, OutOrigin, OutMove)
        {
            Condition = cylinderCondition;
            Delay = cylinderDelay;
        }

        #region "属性"
        public CylinderCondition Condition { get; set; }

        #region 异常报警及提示
        /// <summary>
        /// 无原点信号报警
        /// </summary>
        /// <returns></returns>
        private bool NoOriginAlarm()
        {
            lock (this)
            {
                bool alarmDelayDone = false;
                if (Condition.External.AirSignal && _OutOrigin.Value && !_OutMove.Value && !_InOrigin.Value) cylinderNoOrigin = true;
                else cylinderNoOrigin = false;
                if (cylinderNoOrigin && !Condition.NoOriginShield)
                    if (Delay.AlarmTime <= _watchAlarm[0].ElapsedMilliseconds) alarmDelayDone = true;
                    else alarmDelayDone = false;
                else _watchAlarm[0].Restart();
                //无原点报警
                if ((cylinderNoOrigin && alarmDelayDone) || (_noOriginAlarm && !Condition.External.AlarmReset)) _noOriginAlarm = true;
                else _noOriginAlarm = false;
                return _noOriginAlarm;
            }
        }
        /// <summary>
        /// 无动点信号报警
        /// </summary>
        /// <returns></returns>
        private bool NoMoveAlarm()
        {
            lock (this)
            {
                bool alarmDelayDone = false;
                if (Condition.External.AirSignal && !_OutOrigin.Value && _OutMove.Value && !_InMove.Value) cylinderNoMove = true;
                else cylinderNoMove = false;

                if (cylinderNoMove && !Condition.NoMoveShield)
                    if (Delay.AlarmTime <= _watchAlarm[1].ElapsedMilliseconds) alarmDelayDone = true;
                    else alarmDelayDone = false;
                else _watchAlarm[1].Restart();
                //无动点报警
                if ((cylinderNoMove && alarmDelayDone) || (_noMoveAlarm && !Condition.External.AlarmReset)) _noMoveAlarm = true;
                else _noMoveAlarm = false;
                return _noMoveAlarm;
            }
        }
        /// <summary>
        /// 气缸为Off时，动点信号报警
        /// </summary>
        /// <returns></returns>
        private bool OffMoveAlarm()
        {
            lock (this)
            {
                bool alarmDelayDone = false;
                if (Condition.External.AirSignal && _OutOrigin.Value && !_OutMove.Value && _InMove.Value) cylinderOffMove = true;
                else cylinderOffMove = false;

                if (cylinderOffMove && !Condition.NoMoveShield)
                    if (Delay.AlarmTime <= _watchAlarm[2].ElapsedMilliseconds) alarmDelayDone = true;
                    else alarmDelayDone = false;
                else _watchAlarm[2].Restart();
                //气缸OFF时，动点信号报警
                if ((cylinderOffMove && alarmDelayDone) || (_offMoveAlarm && !Condition.External.AlarmReset)) _offMoveAlarm = true;
                else _offMoveAlarm = false;
                return _offMoveAlarm;
            }
        }
        /// <summary>
        /// 气缸为On时，原点信号报警
        /// </summary>
        /// <returns></returns>
        private bool OnOriginAlarm()
        {
            lock (this)
            {
                bool alarmDelayDone = false;
                if (Condition.External.AirSignal && !_OutOrigin.Value && _OutMove.Value && _InOrigin.Value) cylinderOnOrigin = true;
                else cylinderOnOrigin = false;

                if (cylinderOnOrigin && !Condition.NoOriginShield)
                    if (Delay.AlarmTime <= _watchAlarm[3].ElapsedMilliseconds) alarmDelayDone = true;
                    else alarmDelayDone = false;
                else _watchAlarm[3].Restart();
                //气缸ON时，原点信号报警
                if ((cylinderOnOrigin && alarmDelayDone) || (_onOriginAlarm && !Condition.External.AlarmReset)) _onOriginAlarm = true;
                else _onOriginAlarm = false;
                return _onOriginAlarm;
            }
        }
        /// <summary>
        /// 手动时，自动模式为ON提示
        /// </summary>
        private bool ManualOnPrompt()
        {
            lock (this)
            {
                //手动操作时，自动为ON提示
                if (Condition.External.ManualMode && _autoExecute && ((bool)Condition.IsOnCondition
                || (bool)Condition.IsOffCondition)) return true;
                else return false;
            }
        }
        /// <summary>
        /// 自动时，手动模式为ON提示
        /// </summary>
        private bool AutoOnPrompt()
        {
            lock (this)
            {
                //自动操作时，手动为ON提示
                if (Condition.External.AutoMode && Condition.External.InitializingDone && cylinderManual)
                    return true;
                else return false;
            }
        }
        #endregion

        #region "状态"
        /// <summary>
        /// 报警可复位
        /// </summary>
        public override bool HaveAlarmReset
        {
            get
            {
                lock (this)
                {
                    //报警取消时，报警可复位提示
                    if ((_noOriginAlarm && (!cylinderNoOrigin))
                        || (_noMoveAlarm && (!cylinderNoMove))
                        || (_offMoveAlarm && (!cylinderOffMove))
                        || (_onOriginAlarm && (!cylinderOnOrigin)))
                        return true;
                    else return false;
                }
            }
        }
        /// <summary>
        /// 动点信号状态
        /// </summary>
        public override bool OutMoveStatus
        {
            get
            {
                lock (this)
                {
                    //动点状态输出
                    if (!_OutOrigin.Value && ((_InMove.Value && !Condition.NoMoveShield)
                    || Condition.NoMoveShield))
                        if (Delay.MoveTime <= _watchMove.ElapsedMilliseconds) return true;
                        else return false;
                    else
                    {
                        _watchMove.Restart();
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// 原点信号状态
        /// </summary>
        public override bool OutOriginStatus
        {
            get
            {
                lock (this)
                {
                    //原点状态输出
                    if (!_OutMove.Value && ((_InOrigin.Value && !Condition.NoOriginShield)
                    || Condition.NoOriginShield))
                        if (Delay.OriginTime <= _watchOrigin.ElapsedMilliseconds) return true;
                        else return false;
                    else
                    {
                        _watchOrigin.Restart();
                        return false;
                    }
                }
            }
        }
        #endregion

        #region "延时"
        public CylinderDelay Delay { get; set; }
        #endregion
        /// <summary>
        /// 气缸报警集合
        /// </summary>
        public IList<Alarm> Alarms
        {
            get
            {
                var list = new List<Alarm>();
                list.Add(new Alarm(() => NoOriginAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}无原点报警！", Name) });
                list.Add(new Alarm(() => NoMoveAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}无动点报警！", Name) });
                list.Add(new Alarm(() => OnOriginAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}ON时，原点信号报警！", Name) });
                list.Add(new Alarm(() => OffMoveAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}OFF时，动点信号报警！", Name) });
                list.Add(new Alarm(() => ManualOnPrompt()) { AlarmLevel = AlarmLevels.None, Name = string.Format("{0}手动时，自动为ON提示！", Name) });
                list.Add(new Alarm(() => AutoOnPrompt()) { AlarmLevel = AlarmLevels.None, Name = string.Format("{0}自动时，手动为ON提示！", Name) });
                return list;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 模块初始化操作
        /// </summary>
        public override void InitExecute()
        {
            cylinderManual = false;
            cylinderEnable = true;
            CylinderActive();
        }
        /// <summary>
        /// 内部复位操作
        /// </summary>
        public override void InternalReset()
        {
            cylinderManual = false;
            cylinderEnable = false;
            CylinderActive();
        }
        /// <summary>
        /// 手动操作
        /// </summary>
        public override void ManualExecute()
        {
            if (Condition.External.ManualMode)
            {
                if (!cylinderManual)
                {
                    if (((!_autoExecute) && (Condition.IsOnCondition??true))
                        || (_autoExecute && (Condition.IsOffCondition?? true)))
                        cylinderManual = true;
                }
                else
                {
                    if (((!_autoExecute) && (Condition.IsOffCondition ?? true))
                        || (_autoExecute && (Condition.IsOnCondition ?? true)))
                        cylinderManual = false;
                }
            }
            cylinderEnable = true;
            CylinderActive();
        }
        /// <summary>
        /// 自动置位操作
        /// </summary>
        public override void Set()
        {
            if (cylinderManual) return;
            if (Condition.External.AutoMode)
            {
                _autoExecute = true;
            }
            CylinderActive();
        }
        /// <summary>
        /// 自动复位操作
        /// </summary>
        public override void Reset()
        {
            if (cylinderManual) return;
                _autoExecute = false;
                CylinderActive();
        }
        private void CylinderActive()
        {
            if ((((!cylinderManual) && (!_autoExecute))
                || (cylinderManual && _autoExecute)) && cylinderEnable)
            {
                _OutMove.Value = false;
                _OutOrigin.Value = true;
            }

            if (((cylinderManual && (!_autoExecute))
                || ((!cylinderManual) && _autoExecute)) && cylinderEnable)
            {
                _OutOrigin.Value = false;
                _OutMove.Value = true;
            }
        }
        #endregion
    }
}
