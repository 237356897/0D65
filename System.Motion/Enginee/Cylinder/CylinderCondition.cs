using System;
using Motion.Interfaces;
namespace Motion.Enginee
{
    public class CylinderCondition
    {
        private readonly Func<bool> _offcondition;
        private readonly Func<bool> _oncondition;
        public CylinderCondition(Func<bool> offCondition,Func<bool> onCondition )
        {
            _offcondition = offCondition;
            _oncondition = onCondition;
        }
        /// <summary>
        /// 无原点屏蔽
        /// </summary>
        public bool NoOriginShield { get; set; }
        /// <summary>
        /// 无动点屏蔽
        /// </summary>
        public bool NoMoveShield { get; set; }
        ///// <summary>
        ///// 气压信号
        ///// </summary>
        //public bool AirSignal { get; set; }
        ///// <summary>
        ///// 报警复位
        ///// </summary>
        //public bool AlarmReset { get; set; }
        ///// <summary>
        ///// 手动模式
        ///// </summary>
        //public bool ManualMode { get; set; }
        ///// <summary>
        ///// 自动模式
        ///// </summary>
        //public bool AutoMode { get; set; }
        ///// <summary>
        ///// 初始化完成标志
        ///// </summary>
        //public bool InitializingDone { get; set; }
        public External External { get; set; }
        /// <summary>
        /// 为OFF条件
        /// </summary>
        public bool? IsOffCondition {
            get
            {
                try
                {
                    return _offcondition();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 为ON条件
        /// </summary>
        public bool? IsOnCondition
        {
            get
            {
                try
                {
                    return _oncondition();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
