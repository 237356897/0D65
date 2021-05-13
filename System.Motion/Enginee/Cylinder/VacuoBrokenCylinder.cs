using System;
using System.Collections.Generic;
using System.Diagnostics;
using Motion.Interfaces;
namespace Motion.Enginee
{
    public class VacuoBrokenCylinder : Cylinder
    {
        #region "字段"
        private bool _noSensorAlarm;//无动点信号报警
        private bool _offSensorAlarm;//无原点信号报警
        private bool vacuoNoSensor;
        private bool vacuoOffSensor;
        private Sensor _InSensor;
        private Signal _OutVacuo;
        private Signal _OutBroken;
        private bool cylinderManual = false;
        private bool cylinderEnable = false;
        private bool _autoExecute;
        private readonly Stopwatch _watchInhale = new Stopwatch();
        private readonly Stopwatch _watchBroken = new Stopwatch();
        private readonly Stopwatch[] _watchAlarm = new Stopwatch[] { new Stopwatch(), new Stopwatch() };
        #endregion

        public VacuoBrokenCylinder(Sensor InSensor, Signal OutVacuo, Signal OutBroken)
        {
            _InSensor = InSensor;
            _OutVacuo = OutVacuo;
            _OutBroken = OutBroken;
            _watchInhale.Restart();
            _watchBroken.Restart();
        }
        public VacuoBrokenCylinder(CylinderCondition cylinderCondition,VacuoDelay vacuoDelay, Sensor InSensor, Signal OutVacuo, Signal OutBroken)
            : this(InSensor, OutVacuo, OutBroken)
        {
            Condition = cylinderCondition;
            Delay = vacuoDelay;
        }

        #region "属性"

        public CylinderCondition Condition { get; set; }

        #region 异常报警及提示
        /// <summary>
        /// 无感应信号报警
        /// </summary>
        /// <returns></returns>
        private bool NoSensorAlarm()
        {
            lock (this)
            {
                bool alarmDelayDone = false;

                if (Condition.External.AirSignal && _OutVacuo.Value && (!_InSensor.Value)) vacuoNoSensor = true;
                else vacuoNoSensor = false;

                if (vacuoNoSensor && !Condition.NoOriginShield)
                    if (Delay.AlarmTime <= _watchAlarm[0].ElapsedMilliseconds) alarmDelayDone = true;
                    else alarmDelayDone = false;
                else _watchAlarm[0].Restart();
                //无感应信号报警
                if ((vacuoNoSensor && alarmDelayDone) || (_noSensorAlarm && (!Condition.External.AlarmReset))) _noSensorAlarm = true;
                else _noSensorAlarm = false;

                return _noSensorAlarm;
            }
        }
        /// <summary>
        /// 感应信号常量报警
        /// </summary>
        /// <returns></returns>
        private bool OffSensorAlarm()
        {
            lock (this)
            {
                bool alarmDelayDone = false;
                if (Condition.External.AirSignal && !_OutVacuo.Value && _InSensor.Value) vacuoOffSensor = true;
                else vacuoOffSensor = false;

                if (vacuoOffSensor && !Condition.NoOriginShield)
                    if (Delay.AlarmTime >= _watchAlarm[1].ElapsedMilliseconds) alarmDelayDone = true;
                    else alarmDelayDone = false;
                else _watchAlarm[1].Restart();
                //感应信号常量报警
                if ((vacuoOffSensor && alarmDelayDone) || (_offSensorAlarm && !Condition.External.AlarmReset)) _offSensorAlarm = true;
                else _offSensorAlarm = false;

                return _offSensorAlarm;
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
                    if ((_noSensorAlarm && (!vacuoNoSensor))
                    || (_offSensorAlarm && (!vacuoOffSensor)))
                        return true;
                    else return false;
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
                    if ((!_OutVacuo.Value) && ((!_InSensor.Value && !Condition.NoOriginShield)
                    || Condition.NoOriginShield))
                        if (Delay.BrokenTime <= _watchBroken.ElapsedMilliseconds)
                        {
                            _OutBroken.Value = false;
                            return true;
                        }
                        else return false;
                    else
                    {
                        _watchBroken.Restart();
                        return false;
                    }
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
                    if (_OutVacuo.Value && ((_InSensor.Value && !Condition.NoOriginShield)
                    || Condition.NoOriginShield))
                        if (Delay.InhaleTime <= _watchInhale.ElapsedMilliseconds) return true;
                        else return false;
                    else
                    {
                        _watchInhale.Restart();
                        return false;
                    }
                }
            }
        }
        #endregion

        #region "延时"
        public VacuoDelay Delay { get; set; }
        #endregion
        /// <summary>
        /// 气缸报警集合
        /// </summary>
        public IList<Alarm> Alarms
        {
            get
            {
                var list = new List<Alarm>();
                list.Add(new Alarm(() => NoSensorAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}无感应信号报警！", Name) });
                list.Add(new Alarm(() => OffSensorAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}感应信号常量报警！", Name) });
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
                    if (((!_autoExecute) && (Condition.IsOnCondition ?? true))
                        || (_autoExecute && (Condition.IsOffCondition ?? true)))
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
            if (((cylinderManual && (!_autoExecute))
                || ((!cylinderManual) && _autoExecute)) && cylinderEnable)
            {
                _OutVacuo.Value = true;
            }
            else
            {
                _OutVacuo.Value = false;
                _OutBroken.Value = true;
            }
        }
        #endregion
    }
}