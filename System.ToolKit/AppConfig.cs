using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.ToolKit
{
    public class AppConfig
    {
        public static string ConfigPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。xml\\Config.xml");
            }
        }

        public static string PassWordPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。xml\\PassWord.xml");
            }
        }

        public static string MotionTemplatePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "。xml\\MotionTemplate.xml");
            }
        }

        public static string AxisParamPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "。xml\\param204.xml");
            }
        }

        public static string ProjectCongigPath_A
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageA\\ProjectConfig\\ProjectConfig.ini");
            }
        }

        public static string ProjectCongigPath_B
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageB\\ProjectConfig\\ProjectConfig.ini");
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
    }
}
