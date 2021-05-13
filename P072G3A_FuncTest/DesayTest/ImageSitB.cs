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
        public const string str_dll_file = @".\ImageB\DesayTester_B.dll";
        // 
        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowDesayImageDlg")]
        public static extern void ShowDesayImageDlg(bool bSitA);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TestDesayImageDll")]
        public static extern void TestDesayImageDll();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SaveCaptureImage")]
        public static extern void SaveCaptureImage();

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
        public static extern bool DesayTestDark(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart")]
        public static extern bool DesayTestChart();


        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestWhite")]
        public static extern bool DesayTestWhite();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestIRChart")]
        public static extern bool DesayTestIRChart(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowSettingDlg")]
        public static extern void ShowSettingDlg(string szWindowText);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowSetPinDlg")]
        public static extern void ShowSetPinDlg(string szWindowText);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ShowSetDeviceDlg")]
        public static extern void ShowSetDeviceDlg(string szWindowText);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart_SFR")]
        public static extern bool DesayTestChart_SFR(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart_ColorGray")]

        //0:PASS 1:ColorNG 2:灰阶NG -1：其他NG
        public static extern int DesayTestChart_ColorGray(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart_FOV")]
        public static extern bool DesayTestChart_FOV(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart_SNR")]
        public static extern bool DesayTestChart_SNR(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart_Distortion")]
        public static extern bool DesayTestChart_Distortion(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart_Alignment")]
        public static extern bool DesayTestChart_Alignment(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart_TiltRotation")]
        public static extern bool DesayTestChart_TiltRotation(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestWhite_WB")]
        public static extern bool DesayTestWhite_WB(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestWhite_Blemish")]
        public static extern bool DesayTestWhite_Blemish(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestWhite_DefectPixel")]
        public static extern bool DesayTestWhite_DefectPixel(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestWhite_Shading")]
        public static extern bool DesayTestWhite_Shading(StringBuilder szTestData);

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTest_GetFPS")]
        public static extern float DesayTest_GetFPS();

        [DllImport(str_dll_file, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DesayTestChart_ChageView")]
        public static extern int DesayTestChart_ChageView(int ViewType, StringBuilder szTestData);

    }
}
