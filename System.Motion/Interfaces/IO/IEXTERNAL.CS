using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motion.Interfaces
{
    /// <summary>
    /// 外部信号
    /// </summary>
    public interface IExternal
    {        /// <summary>
             /// 气压信号
             /// </summary>
        bool AirSignal { get; set; }
        /// <summary>
        /// 报警复位
        /// </summary>
        bool AlarmReset { get; set; }
        /// <summary>
        /// 手动模式
        /// </summary>
        bool ManualMode { get; set; }
        /// <summary>
        /// 自动模式
        /// </summary>
        bool AutoMode { get; set; }
        /// <summary>
        /// 初始化完成标志
        /// </summary>
        bool InitializingDone { get; set; }
    }
}
