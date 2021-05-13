using System;
using System.Collections.Generic;
using System.Runtime .InteropServices ;
using System.ComponentModel ;
using System.Threading;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace System.ToolKit
{
    /// <summary>
    /// 定时器类
    /// </summary>
    public class CTimer:Stopwatch 
    {
        /// <summary>
        /// 开始计时状态
        /// </summary>
        private bool m_StartTime;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CTimer()
        {
            Reset();
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~CTimer()
        {
            m_StartTime = false;
            Stop();
            Restart();
        }
         /// <summary>
        ///  开始计时
         /// </summary>
        public void StartTime()
        {
            //开始计时
            Reset();
            Start();
            m_StartTime = true;
        }

        /// <summary>
        /// 结束计时
        /// </summary>
        public void StopTime()
        {
            //计时停止
            Stop();
            m_StartTime = false;
        }
        /// <summary>
        /// 返回计时结果
        /// </summary>
        public double Duration
        {
            get
            {
                return (double)ElapsedMilliseconds/1000;
            }
        }
        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="s">延时时间</param>
        /// <returns></returns>
        public bool WaitTime(double s)
        {
            double CurrentTimer = Duration;
            if (m_StartTime)
                if (CurrentTimer >= s)
                {
                    Stop();
                    return true;
                }
                else
                {
                    return false;
                }
            else
                return false;
        }
    }
}
