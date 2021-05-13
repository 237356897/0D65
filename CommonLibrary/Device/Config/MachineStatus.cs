using Motion.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CommonLibrary
{
    public class MachineStatus
    {
        public static Status PreStatus = Status.停止;
        public static Status NewStatus = Status.停止;
        private static IoPoint[] lights;
        private static Stopwatch _watch = new Stopwatch();
        private static bool intervalSign;
        private static bool RedLamp;
        public static bool VoiceClosed;
        public static bool ExitSystem;

        public static void LoadConfig(IoPoint[] outputs)
        {
            if (outputs.Length == 4)
            {
                lights = outputs;
                ExitSystem = false;
                Thread lightStatusThread = new Thread(new ThreadStart(lightStatus));
                lightStatusThread.IsBackground = true;
                lightStatusThread.Start();
            }
            else
            {
                throw new Exception("元素数量必须等于4");
            }            
        }

        #region 设备状态
        public enum Status
        {
            运行ing,
            复位ing,
            复位完成,
            停止,
            暂停,
            急停,
            故障报警
        }
        #endregion

        public static void lightStatus()
        {
            while (!ExitSystem)
            {
                #region 间隔

                _watch.Stop();
                if (intervalSign)
                {
                    if (_watch.ElapsedMilliseconds / 1000.0 > 0.5)
                    {
                        intervalSign = false;
                        _watch.Restart();
                    }
                }
                else
                {
                    if (_watch.ElapsedMilliseconds / 1000.0 > 0.5)
                    {
                        intervalSign = true;
                        _watch.Restart();
                    }
                }
                _watch.Start();

                #endregion

                #region 红灯
                if (intervalSign && (NewStatus == Status.故障报警 || NewStatus == Status.急停))
                {
                    lights[0].Value = true;
                    RedLamp = true;
                }
                else
                {
                    lights[0].Value = false;
                    RedLamp = false;
                }
                #endregion

                #region 蜂鸣器
                if (RedLamp && !VoiceClosed)
                {
                    lights[3].Value = true;
                }
                else
                {
                    lights[3].Value = false;
                }
                #endregion

                #region 黄灯
                if (!RedLamp && (((NewStatus == Status.暂停 || NewStatus == Status.复位ing) && intervalSign)
                    || (NewStatus == Status.复位完成 || NewStatus == Status.停止)))
                {
                    lights[1].Value = true;
                }
                else
                {
                    lights[1].Value = false;
                }
                #endregion

                #region 绿灯
                if (!RedLamp && NewStatus == Status.运行ing)
                {
                    lights[2].Value = true;
                }
                else
                {
                    lights[2].Value = false;
                }
                #endregion
            }
        }

    }
}
