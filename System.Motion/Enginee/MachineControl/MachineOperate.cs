using System;
using Motion.Interfaces;
using System.Diagnostics;
namespace Motion.Enginee
{
    public class MachineOperate
    {
        private readonly Func<bool> m_condition;
        private readonly Func<bool> m_isAlarm;
        private readonly Stopwatch _watch = new Stopwatch();
        private readonly Stopwatch _delay = new Stopwatch();
        private IoPoint m_startSignal, m_pauseSignal, m_estopSignal;
        private LayerLight m_layerLight;
        private bool m_Start, m_Stop, m_Pause, m_Reset, m_ManualAuto, m_IniliazieDone;
        private bool m_Running, m_Pausing, m_Resetting, m_Alarming;
        private bool m_NotReady, m_Ready, m_sign1, m_sign3;
        public MachineOperate(Func<bool> Condition, Func<bool> IsAlarm, IoPoint StartSignal, IoPoint PauseSignal, IoPoint EstopButton,
            LayerLight layerLight)
        {
            m_condition = Condition;
            m_isAlarm = IsAlarm;
            m_startSignal = StartSignal;
            m_pauseSignal = PauseSignal;
            m_estopSignal = EstopButton;
            m_layerLight = layerLight;
            _watch.Start();
            _delay.Start();
        }
        public bool Start { set { m_Start = value; } }
        public bool Stop { set { m_Stop = value; } }
        public bool Pause { set { m_Pause = value; } }
        public bool Reset { set { m_Reset = value; } }
        public bool CleanProductDone { get; set; }
        public bool VoiceClosed { get; set; }
        public bool PlateLittle { get; set; }
        public bool ManualAutoModel { set { m_ManualAuto = value; } }
        public bool IniliazieDone { set { m_IniliazieDone = value; } }
        public bool RunningSign { get; set; }
        public bool Running { get { return m_Running; } }
        public bool Pausing { get { return m_Pausing; } }
        public bool Stopping { get; set; }
        public bool Resetting { get { return m_Resetting; } }
        public bool Alarming { get { return m_Alarming; } }
        public int Flow { get; set; }
        public MachineStatus Status { get; private set; }
        public void Run()
        {
            //获取执行条件
            var _condition = m_condition();
            //获取故障状态
            var _isAlarm = m_isAlarm();

            var _Start = m_Start | m_startSignal.Value;
            var _Pause = m_pauseSignal.Value | m_Pause;
            var _Estop = !m_Reset & m_estopSignal.Value;
            //启动标记
            if (_Start && m_ManualAuto && !_Pause && m_estopSignal.Value && _condition && !_isAlarm && m_IniliazieDone)
                RunningSign = true;
            //启动中
            if ((_Start || m_Running) && m_ManualAuto && !_Pause &&
                m_estopSignal.Value && _condition && !_isAlarm && m_IniliazieDone && RunningSign)
                m_Running = true;
            else
                m_Running = false;

            if (!m_estopSignal.Value || CleanProductDone) RunningSign = false;
            //停止中
            if ((m_Stop || Stopping) && !m_ManualAuto && !m_Reset && !_Start && _condition && !_isAlarm && m_estopSignal.Value)
                Stopping = true;
            else
                Stopping = false;
            //暂停中
            if ((_Pause || _isAlarm || m_Pausing) && m_ManualAuto && !_Start && _condition && m_estopSignal.Value)
                m_Pausing = true;
            else
                m_Pausing = false;

            if (!m_sign3 && m_Reset)
            {
                Flow = 0;
                m_sign3 = true;
            }
            if (!m_Reset) m_sign3 = false;
            if ((m_Reset || m_Resetting) && !m_Stop && !m_IniliazieDone&& !_isAlarm && m_estopSignal.Value)
                m_Resetting = true;
            else
                m_Resetting = false;

            if (_isAlarm) m_Alarming = true;
            else m_Alarming = false;

            if (!m_IniliazieDone && !_isAlarm && m_estopSignal.Value) m_NotReady = true;
            else m_NotReady = false;

            if (m_IniliazieDone && !_isAlarm && !m_Running && !Stopping && !m_Pausing && !m_Resetting)
                m_Ready = true;
            else
                m_Ready = false;

            if (m_NotReady) Status = MachineStatus.设备未准备好;
            if (m_Ready) Status = MachineStatus.设备准备好;
            if (m_Running) Status = MachineStatus.设备运行中;
            if (Stopping) Status = MachineStatus.设备停止中;
            if (m_Pausing && !_isAlarm) Status = MachineStatus.设备暂停中;
            if (m_Resetting) Status = MachineStatus.设备复位中;
            if (m_Alarming) Status = MachineStatus.设备故障中;
            if (!m_estopSignal.Value) Status = MachineStatus.设备急停已按下;
            if (m_Running) m_layerLight.GreenLamp.Value = true;
            else m_layerLight.GreenLamp.Value = false;

            _watch.Stop();
            if (m_sign1)
            {
                if (_watch.ElapsedMilliseconds / 1000.0 > 0.5)
                {
                    m_sign1 = false;
                    _watch.Restart();
                }
            }
            else
            {
                if (_watch.ElapsedMilliseconds / 1000.0 > 0.5)
                {
                    m_sign1 = true;
                    _watch.Restart();
                }
            }
            _watch.Start();
            if (((Stopping || (m_Pausing && !_isAlarm) || PlateLittle) && m_sign1)
                || (m_IniliazieDone && !_isAlarm && !m_Running && !Stopping && !m_Pausing))
                m_layerLight.YellowLamp.Value = true;
            else
                m_layerLight.YellowLamp.Value = false;

            if (m_Alarming || m_NotReady) m_layerLight.RedLamp.Value = true;
            else m_layerLight.RedLamp.Value = false;

            _delay.Stop();
            if (m_Alarming && !VoiceClosed)
            {
                if (m_sign1) m_layerLight.Speeker.Value = true;
                else m_layerLight.Speeker.Value = false;
            }
            else
            {
                m_layerLight.Speeker.Value = false;
                _delay.Restart();
            }
            _delay.Start();
        }
    }
}
