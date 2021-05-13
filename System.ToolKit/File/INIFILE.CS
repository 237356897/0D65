using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
//XML引用
using System.Xml;
//序列化引用
using System.Runtime.Serialization.Formatters.Binary;
namespace System.ToolKit
{
    public static class IniFile
    {
        #region INI文件操作
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        //函数作用：向INI文件中写信息


        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        //函数作用：从私有文件中获取字符串（读Ini文件）
        //section:欲在其中查找条目的小节名称
        //key:欲获取的项名或条目名
        //def:指定的条目没有找到时返回的默认值。可设为空（""） 
        //retVal:指定一个字串缓冲区，长度至少为size 
        //size:缓冲区大小
        //filePath:INI文件的完整路径

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSection")]
        private static extern uint GetPrivateProfileSection(string section, sbyte[] lpReturnedString, uint nSize, string lpFileName);
        //函数作用：向INI文件中获取指定段落的项名称


        /// <summary>
        /// 写ini文件函数
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">关键字</param>
        /// <param name="Value">要设置的值</param>
        /// <param name="filepath">ini文件路径</param>
        public static long WriteValue(string Section, string Key, string Value, string filepath)//对ini文件进行写操作的函数
        {
            long r = WritePrivateProfileString(Section, Key, Value, filepath);
            return r;
        }
        /// <summary>
        /// 读ini文件函数
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">关键字</param>
        /// <param name="filepath">文件路径</param>
        /// <returns>返回string</returns>
        public static T ReadValue<T>(string Section, string Key, string filepath, T DefaultValue)//对ini文件进行读操作的函数
        {
            StringBuilder temp = new StringBuilder(255);
            long i = GetPrivateProfileString(Section, Key, null, temp, 255, filepath);
            if (temp.ToString().Trim() == "" || temp.ToString() == null) return DefaultValue;
            return (T)Convert.ChangeType(temp.ToString(), typeof(T));
        }

        /// <summary>
        /// 获取ini文件中指定段落的项名称
        /// </summary>
        /// <param name="iniPath">文件路径</param>
        /// <param name="section">指定的段落</param>
        /// <param name="encoding">文字编码方式</param>
        /// <returns>返回段落的项名称集合</returns>
        public static List<string> GetSectionItems(string iniPath, string section, Encoding encoding = null)
        {
            const uint StringBufferSize = 32768;
            Encoding Enc = Encoding.Default;

            if (encoding == null) { encoding = Enc; }
            sbyte[] lpReturnedString = new sbyte[StringBufferSize];
            uint len = GetPrivateProfileSection(section, lpReturnedString, (uint)lpReturnedString.Length, iniPath);

            List<string> items = new List<string>();
            unsafe
            {
                fixed (sbyte* pBuf = lpReturnedString)
                {
                    uint i = 0;
                    while (i < len)
                    {
                        uint j;
                        uint start = j = i;
                        uint length = 0;

                        while (i < len && lpReturnedString[i] != 0)
                        {
                            ++i;
                        }
                        while (j < i && lpReturnedString[j] != 61)
                        {
                            ++j;
                        }
                        length = j - start;
                        items.Add(new string(pBuf, (int)start, (int)length, encoding));
                        ++i;
                        //if (i < len && lpReturnedString[i] == 0)
                        //{
                        //    break;
                        //}
                    }
                }
            }

            return items;
        }
        #endregion

        #region  序列化

        //注意：序列化需要在需序列化的对像前设置属性，如下所示
        //[Serializable]
        //private struct structTest { public byte one; public int two; public string three; }

        /// <summary>
        /// 序列化至TXT文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="source">序列化对象</param>
        /// <param name="mode">指定操作系统打开文件的方式</param>
        /// <param name="access">定义用于控制对文件的读访问、写访问或读/写访问的常数</param>
        /// <param name="share">包含用于控制其他 System.IO.FileStream 对象对同一文件可以具有的访问类型的常数</param>
        /// <returns></returns>
        public static void SerializeToTxt(string path, object source, FileMode mode, FileAccess access, FileShare share)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writeFileStream = new FileStream(path, mode, access, share);
            formatter.Serialize(writeFileStream, source);
            writeFileStream.Close();
        }

        /// <summary>
        /// 从TXT文件反序列化
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="mode">指定操作系统打开文件的方式</param>
        /// <param name="access">定义用于控制对文件的读访问、写访问或读/写访问的常数</param>
        /// <param name="share">包含用于控制其他 System.IO.FileStream 对象对同一文件可以具有的访问类型的常数</param>
        /// <returns></returns>
        public static object DeserializationFromTxt(string path, FileMode mode, FileAccess access, FileShare share)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream readFileStream = new FileStream(path, mode, access, share);
            object temp = (object)formatter.Deserialize(readFileStream);
            readFileStream.Close();
            return temp;
        }

        #endregion
    }
}
