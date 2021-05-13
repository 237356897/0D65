using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace desay.ProductData
{
    public class AppConfig
    {
        public static string TestConfigPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。xml\\Config.xml");
            }
        }

        public static string ConfigFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。ini\\Config.ini");
            }
        }

        public static string xmlTemplatePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。xml\\AxisTemplate.xml");
            }
        }

        public static string LoadMotionCardParameterFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。xml\\param204.xml");
            }
        }

        public static string LogFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs\\" + DateTime.Now.ToString("yyyyMM") + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
            }
        }

        public static string ProductDataFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。ini\\ProductData.ini");
            }
        }

        public static string SpecFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。ini\\Spec.ini");
            }
        }
        /// <summary>
        /// 生产信息文件路径
        /// </summary>
        public static string ProdutionInfoFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。ini\\ProdutionInfo.ini");
            }
        }
        /// <summary>
        /// MES配置参数文件路径
        /// </summary>
        public static string MESConfigFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。ini\\MESConfig.ini");
            }
        }
        /// <summary>
        /// 本地数据库文件夹路径
        /// </summary>
        public static string dataBaseDirectoryPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"DataBase");
            }
        }
        /// <summary>
        /// 本地数据库文件路径
        /// </summary>
        public static string dataBaseFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.db3", DateTime.Now.ToString("yyyyMMdd")));
            }
        }
    }
}
