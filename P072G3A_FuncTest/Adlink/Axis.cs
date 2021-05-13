using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motion.AdlinkAps;

namespace P072G3A_FuncTest
{
    public class Axis
    {
        public int MaxVel;


        private int axisNo;
        private ApsController motionCard;
        private bool isServoON = true;//初始化默认为ON，轴卡加载的时候已全部ON

        public Axis(ApsController card,int axisNum)
        {
            axisNo = axisNum;
            motionCard = card;
        }

        #region Public Method             
        /// <summary>
        /// 设置单轴加减速时间
        /// </summary>
        /// <param name="accVel">加速时间</param>
        /// <param name="decVel">减速时间</param>
        /// <param name="maxVel">最大速度</param>
        public void SetVelParam(double accVel,double decVel,int maxVel)
        {
            motionCard.SetAxisAccDecVel(axisNo, accVel, decVel);
            MaxVel = maxVel;
        }

        /// <summary>
        /// 单轴软限位配置
        /// </summary>
        /// <param name="softPositiveLimit"></param>
        /// <param name="softNegativeLimit"></param>
        public void SetSoftConfig(double softPositiveLimit,double softNegativeLimit)
        {
            motionCard.SetSoftConfig(axisNo, softPositiveLimit, softNegativeLimit);
        }

        /// <summary>
        /// 取消单轴软限位配置
        /// </summary>
        public void ClearSoftConfig()
        {
            motionCard.ClearSoftConfig(axisNo);
        }

        /// <summary>
        /// 回零位
        /// </summary>
        public void BackHome()
        {
            motionCard.BackHome(axisNo);
        }

        /// <summary>
        /// 单轴相对运动
        /// </summary>
        /// <param name="pulseNum"></param>
        /// <param name="maxVel"></param>
        public void MoveRel(int pulseNum, int maxVel)
        {
            motionCard.MoveRelPulse(axisNo, pulseNum, maxVel);
        }

        /// <summary>
        /// 单轴绝对运动
        /// </summary>
        /// <param name="pulseNum">脉冲数</param>
        /// <param name="maxVel">最大速度</param>
        public void MoveAbs(int pulseNum, int? maxVel = null)
        {
            int tempMaxVel;
            if (maxVel != null)
            { tempMaxVel = (int)maxVel; }
            else
            { tempMaxVel = MaxVel; }

            motionCard.MoveAbsPulse(axisNo, pulseNum, tempMaxVel);
        }

        /// <summary>
        /// 连续运动
        /// </summary>
        /// <param name="moveDirection"></param>
        /// <param name="maxVel"></param>
        public void MoveContinu(MoveDirection moveDirection, int maxVel)
        {
            motionCard.MoveContinu(axisNo, moveDirection, maxVel);
        }

        /// <summary>
        /// JOG运动
        /// </summary>
        /// <param name="isOn">0=OFF,1=ON</param>
        public void MoveJog(int isOn)
        {
            motionCard.MoveJog(axisNo, isOn);
        }

        /// <summary>
        /// 单轴急停
        /// </summary>
        public void EmergencyStop()
        {
            motionCard.EmergencyStop(axisNo);
        }

        /// <summary>
        /// 单轴减速停止
        /// </summary>
        public void DecelStop()
        {
            motionCard.DecelStop(axisNo);
        }

        /// <summary>
        /// 是否停止移动
        /// </summary>
        public bool IsMotionless
        {
            get{ return motionCard.IsMotionless(axisNo); } 
        }

        public bool IsPosComplete(int pos)
        {
            return IsMotionless && (GetCurrentCommandPosition == pos);
        }

        /// <summary>
        /// 是否已伺服ON
        /// </summary>
        /// <param name="axisNo"></param>
        public bool IsServoON
        {
            get { return isServoON; }
            set
            {
                if (isServoON = value)
                { motionCard.ServoOn(axisNo); }
                else
                { motionCard.ServoOff(axisNo); }
            }
        }

        /// <summary>
        /// 是否在报警状态
        /// </summary>
        public bool IsAlarm
        {
            get { return motionCard.IsAlarm(axisNo); }
        }

        /// <summary>
        /// 是否在急停状态
        /// </summary>
        public bool IsEmg
        {
            get { return motionCard.IsEmg(axisNo); }
        }

        /// <summary>
        /// 是否在轴原点
        /// </summary>
        public bool IsOrg
        {
            get { return motionCard.IsOrg(axisNo); }
        }

        /// <summary>
        /// 是否到达正限位
        /// </summary>
        public bool IsPel
        {
            get { return motionCard.IsPel(axisNo); }
        }

        /// <summary>
        /// 是否到达负限位
        /// </summary>
        public bool IsMel
        {
            get { return motionCard.IsMel(axisNo); }
        }

        /// <summary>
        /// 获取当前位置
        /// </summary>
        public int GetCurrentCommandPosition
        {
            get { return motionCard.GetCurrentCommandPosition(axisNo); }
        }

        /// <summary>
        /// 获取当前速度
        /// </summary>
        public int GetCurrentCommandSpeed
        {
            get { return motionCard.GetCurrentCommandSpeed(axisNo); }
        }
        #endregion
    }
}
