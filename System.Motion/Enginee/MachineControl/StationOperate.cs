using System;
using System.Diagnostics;

namespace Motion.Enginee
{
    public class StationOperate : IStationOperate
    {
        #region 字段
        private bool m_StartForbid;
        private bool m_Start;
        private bool m_JogStartForbid;
        private bool m_JogStart;
        private bool m_Stop;
        private bool m_Reset;
        private bool m_manualMode;
        private bool m_autoMode;
        private bool m_autoRun;
        private bool m_RunSignal;
        private bool m_SingleRunning;
        private bool _SingleRunning;
        private bool _JogRunning;
        private bool m_Running;
        private StationStatus m_Status;
        private bool m_conditionOK;
        private readonly Func<bool> m_condition;
        private readonly Func<bool> m_isAlarm;
        private readonly Stopwatch _watch = new Stopwatch();
        #endregion
        public StationOperate(Func<bool> Condition, Func<bool> IsAlarm)
        {
            m_condition = Condition;
            m_isAlarm = IsAlarm;
            _watch.Restart();
        }
        #region 属性
        /// <summary>
        /// 单动
        /// </summary>
        public bool Start { set { m_Start = value; } }
        /// <summary>
        /// 点动
        /// </summary>
        public bool JogStart { set { m_JogStart = value; } }
        /// <summary>
        /// 暂停
        /// </summary>
        public bool Stop { set { m_Stop = value; } }
        /// <summary>
        /// 复位
        /// </summary>
        public bool Reset { set { m_Reset = value; } }
        /// <summary>
        /// 手动模式
        /// </summary>
        public bool ManualMode { set { m_manualMode = value; } }
        /// <summary>
        /// 自动模式
        /// </summary>
        public bool AutoMode { set { m_autoMode = value; } }
        /// <summary>
        /// 自动运行
        /// </summary>
        public bool AutoRun { set { m_autoRun = value; } }
        /// <summary>
        /// 单动运行中
        /// </summary>
        public bool SingleRunning
        {
            get { return m_SingleRunning; }
        }
        /// <summary>
        /// 运行中
        /// </summary>
        public bool Running { get { return m_Running; } }
        /// <summary>
        /// 模组状态
        /// </summary>
        public StationStatus Status { get { return m_Status; } }

        public bool RunningSign
        {
            set
            {
                m_RunSignal = value;
            }
        }
        #endregion
        public void Run()
        {
            //获取执行条件
            var _condition = m_condition();
            //获取故障状态
            var _isAlarm = m_isAlarm();
            //判断准备好状态
            if (_condition && !_isAlarm && m_manualMode && !m_autoRun)
            {
                if (2000 <= _watch.ElapsedMilliseconds)
                {
                    m_conditionOK = true;
                }
                else
                {
                    m_conditionOK = false;
                }
            }
            else
            {
                _watch.Restart();
            }
            //单动上升沿触发
            if (!m_StartForbid && m_Start)
            {
                if (m_conditionOK) m_RunSignal = true; ;
                m_StartForbid = true;
            }
            if (!m_Start) m_StartForbid = false;
            //寸动上升沿触发
            if (!m_JogStartForbid && m_JogStart)
            {
                if (m_conditionOK) m_RunSignal = true; ;
                m_JogStartForbid = true;
            }
            if (!m_JogStart) m_JogStartForbid = false;
            if (m_autoMode || m_Reset) m_RunSignal = false;

            //判断执行单动控制
            if ((m_Start || _SingleRunning) && !_isAlarm && m_manualMode && !m_autoRun && m_RunSignal && !m_Stop) _SingleRunning = true;
            else _SingleRunning = false;
            //判断执行点动控制
            if (m_JogStart && !_isAlarm && m_manualMode && !m_autoRun && m_RunSignal) _JogRunning = true;
            else _JogRunning = false;

            if (_SingleRunning || _JogRunning) m_SingleRunning = true;
            else m_SingleRunning = false;

            if (m_SingleRunning || m_autoRun) m_Running = true;
            else m_Running = false;

            //设备状态
            if (_isAlarm) m_Status = StationStatus.模组报警;
            if (!_isAlarm && !m_RunSignal && !_condition) m_Status = StationStatus.模组未准备好;
            if (!_isAlarm && !m_RunSignal && _condition && !m_Running) m_Status = StationStatus.模组准备好;
            if (!_isAlarm && m_RunSignal && m_Running) m_Status = StationStatus.模组运行中;
            if (!_isAlarm && m_RunSignal && !m_Running) m_Status = StationStatus.模组暂停中;
        }
    }
}
