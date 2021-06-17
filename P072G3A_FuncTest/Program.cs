using desay.ProductData;
using P072G3A_FuncTest.Data;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Toolkit.Helpers;
using System.ToolKit;
using System.Windows.Forms;

namespace P072G3A_FuncTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isRunning;
            Mutex mutex = new Mutex(true, "RunOneInstanceOnly", out isRunning);
            if (isRunning)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    Config.Instance = SerializerManager<Config>.Instance.Load(AppConfig.ConfigPath);
                }
                catch { MessageBox.Show("Config.xml出错!"); Application.Exit(); }
                try
                {
                    Position.Instance = SerializerManager<Position>.Instance.Load(Marking.PositionPath);
                }
                catch { MessageBox.Show("TestItem.xml出错!"); Application.Exit(); }
                try
                {
                    Global.Instance.PassWord = SerializableDictionary<string, string>.Instance.LoadDicXml();
                }
                catch { MessageBox.Show("PassWord.xml出错!"); Application.Exit(); }

                //SerializerManager<Config>.Instance.Save(AppConfig.ConfigPath,Config.Instance);
                //SerializerManager<Position>.Instance.Save(Marking.PositionPath,Position.Instance);
                //SerializableDictionary<string, string>.Instance.SaveDicXml(Global.Instance.PassWord);

                //Application.Run(new frmYield(false));
                Application.Run(new frmMain());
            }
            else
            {
                MessageBox.Show("程序已经启动！");
            }

        }
    }
}
