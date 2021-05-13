using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;  
using System.IO;

namespace System.ToolKit
{
    public class ConsoleShow
    {
        /// <summary>
        /// 启动控制台
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        /// <summary>
        /// 释放控制台
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //*************************************************************************

        public static void Init()
        {
            AllocConsole();
//            Visible(false, Console.Title);
            DisableCloseButton(Console.Title);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Beep();
            //Console.WriteLine(Console.Title.ToString());
            Console.WriteLine("* Console program boot ok!");
        }

        public static void DisableCloseButton(string title)
        {
            Thread.Sleep(100);

            IntPtr windowHandle = FindWindow(null, title);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }

        public static void Visible(bool visible, string title)
        {
            // below is Brandon's code                 
            IntPtr hWnd = FindWindow(null, title);

            if (hWnd != IntPtr.Zero)
            {
                if (!visible)
                    //Hide the window                   
                    ShowWindow(hWnd, 0); // 0 = SW_HIDE               
                else
                    //Show window again                   
                    ShowWindow(hWnd, 1); //1 = SW_SHOWNORMA          
            }
        }
    }
}
