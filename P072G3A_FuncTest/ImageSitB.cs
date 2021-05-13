using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Image_Sitenamespace
{
    class ImageSitB
    {
        public const string str_dll_file = @".\ImageB\DesayTester.dll";
        // 
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowDesayImageDlg")]
        public static extern void ShowDesayImageDlg();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TestDesayImageDll")]
        public static extern void TestDesayImageDll();


        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ExitDesayImageDlg")]
        public static extern void ExitDesayImageDlg();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDesayImageDlgHwnd")]
        public static extern IntPtr GetDesayImageDlgHwnd();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "PlayCamera")]
        public static extern bool PlayCamera();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "StopCamera")]
        public static extern bool StopCamera();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetBarcode")]
        public static extern void SetBarcode(string szBarcode, string szJigSN);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestDark")]
        public static extern bool DesayTestDark();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart")]
        public static extern bool DesayTestChart();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestWhite")]
        public static extern bool DesayTestWhite();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestIRChart")]
        public static extern bool DesayTestIRChart();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowSettingDlg")]
        public static extern void ShowSettingDlg(string szWindowText);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowSetPinDlg")]
        public static extern void ShowSetPinDlg(string szWindowText);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowSetDeviceDlg")]
        public static extern void ShowSetDeviceDlg(string szWindowText);

    }
}
