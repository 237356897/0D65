using desay.ProductData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace P072G3A_FuncTest
{
    class Marking
    {
        /// <summary>
        ///  光源1数据
        /// </summary>
        public static string[] LightSource1Data;

        /// <summary>
        ///  光源2数据
        /// </summary>
        public static string[] LightSource2Data;

        /// <summary>
        ///  光源3数据
        /// </summary>
        public static string[] LightSource3Data;

        /// <summary>
        ///  光源4数据
        /// </summary>
        public static string[] LightSource4Data;

        /// <summary>
        ///  光源5数据
        /// </summary>
        public static string[] LightSource5Data;


        /// <summary>
        ///  光源6数据---白场
        /// </summary>
        public static string[] LightSource6Data;


        /// <summary>
        ///  电流数据
        /// </summary>
        public static string[] currentSource7Data;

        /// <summary>
        ///  光源1接收数据完成
        /// </summary>
        public static bool isLightSource1Completed;
        /// <summary>
        ///  光源2接收数据完成
        /// </summary>
        public static bool isLightSource2Completed;
        /// <summary>
        ///  光源3接收数据完成
        /// </summary>
        public static bool isLightSource3Completed;
        /// <summary>
        ///  光源4接收数据完成
        /// </summary>
        public static bool isLightSource4Completed;
        /// <summary>
        ///  光源5接收数据完成
        /// </summary>
        public static bool isLightSource5Completed;

        /// <summary>
        ///  光源6接收数据完成
        /// </summary>
        public static bool isLightSource6Completed;


        /// <summary>
        ///  电流接收数据完成
        /// </summary>
        public static bool isCurrentSource7Completed;

        /// <summary>
        /// 设备选择手动模式
        /// </summary>
        public static bool ManualMode = true;

        /// <summary>
        /// Position路径
        /// </summary>
        public static string PositionPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"。xml\\TestItem\\{Config.Instance.CurrentProduct}.xml");
            }
        }

    }
}
