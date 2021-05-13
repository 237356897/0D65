using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Net.Sockets;

namespace CommonLibrary
{
    #region EventArgs
    /// <summary>
    /// 包含事件方法的类，向此类实例化对象赋予函数
    /// </summary>
    public class TriggerStartingEventArgs : EventArgs
    {
        /// <summary>
        /// 获取或设置重试计数
        /// </summary>
        public int TryCount { get; set; }

        /// <summary>
        /// 获取或设置触发时的命令
        /// </summary>
        public string Command { get; set; }
    }

    #endregion

    public class Common
    {
        #region Delegate
        public delegate void void_SocketDelegate(Socket socket);
        public delegate void void_SocketStringDelegate(Socket socket, string str);
        public delegate void void_ObjectBoolDelegate(object obj, bool b);
        public delegate void void_StringTriggerDelegate(string str, TriggerStartingEventArgs trg);     
        public delegate void void_StringDelegate(string str);
        public delegate void void_BoolDelegate(bool b);
        public delegate void ExecuteDelegate();

        public delegate void void_StringListDelegate(List<string> strs);

        public delegate int int_DynamicDelegate(dynamic dy);
        public delegate bool bool_StringDelegate(string str);
        public delegate bool bool_bool_Delegate(bool? b);

        public delegate string string_StringDelegate(string str);
       
        public delegate int TypeTransmit_int_DynamicDelegate<T>(dynamic txt);
        #endregion

        #region Common Config

        #region KeyPress
        /// <summary>
        /// 限制键盘按键只能是数字输入与 keyChar 字符参数(默认是删除退格键)
        /// </summary>
        /// <param name="obj">触发此方法的某对象</param>
        /// <param name="e">触发此方法的某事件</param>
        /// <param name="keyChar">开放允许按键的某个字符（可以不给此参数）</param>
        public static void IsDigitalInput(object obj, KeyPressEventArgs e, char keyChar = (char)8)
        {
            if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == keyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 限制键盘按键只能是 keyChar 参数指定的Ascii字符（默认是回车键）
        /// </summary>
        /// <param name="obj">触发此方法的某对象</param>
        /// <param name="e">触发此方法的某事件</param>
        /// <param name="keyChar">允许按键的某个字符（默认是回车键）</param>
        /// <returns>如果按键匹配则返回True，否则False</returns>
        public static bool IsKey(object obj, KeyPressEventArgs e, char keyChar = (char)13)
        {
            bool isPass = false;
            if (e.KeyChar == keyChar)
            {
                isPass = true;
            }
            return isPass;
        }

        #endregion

        #endregion

        #region Common File
        public static string FileDelete(string filePath)
        {
            string strRet = "";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                strRet = "TRUE";
            }
            else
            {
                strRet = string.Format("删除失败：{0}文件不存在!", filePath);
            }

            return strRet;
        }

        public static string FileCreate(string filePath)
        {
            string strRet = "";
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Directory.Exists)
            {//创建目录
                fi.Directory.Create();
            }
            if (!File.Exists(filePath))
            {            
                File.Create(filePath).Close();
                strRet = "TRUE";
            }
            else
            {
                strRet = string.Format("创建失败：{0}文件已存在！", filePath);
            }

            return strRet;
        }

        #endregion

    }
}
