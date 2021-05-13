using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace desay.ProductData
{
    [Serializable]
    public class Config
    {
        [NonSerialized]
        public static Config Instance = new Config();

        #region UI位置
        public struct ResultPos
        {
            public Point FirstPoint_A;
            public Point FirstPoint_B;
            public Point LabFirstPoint_A;
            public Point LabFirstPoint_B;
            public int PicWidth;
            public int PicHeight;
            public int LabWidth;
            public int LabHeight;

        };
        public ResultPos RPos = new ResultPos();

        #endregion

        #region 屏蔽功能
        /// <summary>
        /// 安全门启用
        /// </summary>
        public bool IsSafetyDoor;
        /// <summary>
        /// 左工位启用
        /// </summary>
        public bool IsLeftWork;
        /// <summary>
        /// 右工位启用
        /// </summary>
        public bool IsRightWork;
        /// <summary>
        /// 空跑模式
        /// </summary>
        public bool IsDryRunMode;
        /// <summary>
        /// 光栅启用
        /// </summary>
        public bool IsGuanShan;
        /// <summary>
        /// 扫码启用
        /// </summary>
        public bool IsScan;
        /// <summary>
        /// NG料筒启用
        /// </summary>
        public bool IsNGBox;
        /// <summary>
        /// 蜂鸣启用
        /// </summary>
        public bool IsSpeak;

        #endregion

        #region 生产信息

        public string ProductCountDis = "0";

        public string OKCountDis = "0";

        public string NGCountDis = "0";

        #endregion

        #region 设备配置
        /// <summary>
        /// 电流Com
        /// </summary>
        public string CurrentCom = "COM15";
        /// <summary>
        /// 扫码器
        /// </summary>
        public string ScanCom = "COM5";
        /// <summary>
        /// 波特率
        /// </summary>
        public string BaudRate = "115200";
        /// <summary>
        /// 字节数
        /// </summary>
        public string DataBit = "8bit";
        /// <summary>
        /// 停止位
        /// </summary>
        public string StopBit = "1bit";
        /// <summary>
        /// 校验位
        /// </summary>
        public string ParityBit = "None无";

        public string Timeout = "3000";

        public string Retries = "3";

        public string Command = "+";

        public string Type = "DatalogicScan";

        public string TestDirectory = @"。xml\TestItem";

        public string MotionDirectory = @"。xml\MotionParam";

        public string[] ProductList = { "SMC00", "XSFH2", "FreeTech" };

        public string CurrentProduct = "FreeTech";

        #endregion

        #region 气缸延时
        /// <summary>
        /// 左黑场升降气缸上延时
        /// </summary>
        public string LeftBlackCYUpDelay;
        /// <summary>
        /// "左黑场升降气缸下延时
        /// </summary>
        public string LeftBlackCYDownDelay;
        /// <summary>
        /// 右黑场升降气缸上延时
        /// </summary>
        public string RightBlackCYUpDelay;
        /// <summary>
        /// 右黑场升降气缸下延时
        /// </summary>
        public string RightBlackCYDownDelay;
        /// <summary>
        /// 白场升降气缸上延时
        /// </summary>
        public string WhiteCYUpDelay;
        /// <summary>
        /// 白场升降气缸下延时
        /// </summary>
        public string WhiteCYDownDelay;
        /// <summary>
        /// 左平移气缸缩回延时
        /// </summary>
        public string LeftSideswayCYReturnDelay;
        /// <summary>
        /// 左平移气缸伸出延时
        /// </summary>
        public string LeftSideswayCYReachDelay;
        /// <summary>
        /// 右平移气缸缩回延时
        /// </summary>
        public string RightSideswayCYReturnDelay;
        /// <summary>
        /// 右平移气缸伸出延时
        /// </summary>
        public string RightSideswayCYReachDelay;
        /// <summary>
        /// 图卡测试延时
        /// </summary>
        public int bDelayTimeValue;
        #endregion

    }
}
