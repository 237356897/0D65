using P072G3A_FuncTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P072G3A_FuncTest
{
    public class Global
    {
        #region 单例

        public static Global Instance = new Global();

        private Global() { }

        #endregion

        //通信相关
        public string ComPort1ComParam = "COM3,9600,None,8,One,1500,1500";
        public string ComPort2ComParam = "COM4,9600,None,8,One,1500,1500";
        public string ComPort3ComParam = "COM5,9600,None,8,One,1500,1500";
        public string ComPort4ComParam = "COM14,9600,None,8,One,1500,1500";
        public string ComPort5ComParam = "COM16,9600,None,8,One,1500,1500";

        public string ComPort6ComParam = "COM6,9600,None,8,One,1500,1500"; //白场光源
        public string ComPort7ComParam = "COM8,9600,None,8,One,1500,1500"; //  电流读取

        public SerializableDictionary<string, string> PassWord = new SerializableDictionary<string, string>();

    }
}
