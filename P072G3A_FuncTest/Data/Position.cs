using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace desay.ProductData
{
    [Serializable]
    public class Position
    {
        [NonSerialized]
        public static Position Instance = new Position();

        #region 测试项
        public struct TestItemConfigParam
        {
            public bool bBlackEN;
            public bool bInfraredDarkEN;
            public bool bBlemishEN;
            public bool bBadPixelEN;
            public bool bHotPixelEN;
            public bool bWBEN;
            public bool bMTFEN;
            public bool bIRMTFEN;
            public bool bShadingEN;
            public bool bGrayEN;
            public bool bColorEN;
            public bool bFOVEN;
            public bool bFPSEN;
            public bool bAlignmentEN;
            public bool bDistortionEN;
            public bool bFWEN;
            public bool bVoltageEN;
            public bool bCurrentEN;
            public bool bPowerEN;            
            public bool bSNREN;
            public bool bRotationEN;
            public bool bChangeViewEN;

            public bool bAllTestEN;

            public float bCurrentMinValue;
            public float bCurrentMaxValue;
            public float bVoltageMinValue;
            public float bVoltageMaxValue;
            public float bPowerMinValue;
            public float bPowerMaxValue;
            public float bFPSMinValue;
            public float bFPSMaxValue;
            public float bFWValue;
            //三个坏境
            public bool bDark_Test;
            public bool bLight_Test;
            public bool bMTF_Test;
        };

        public TestItemConfigParam testItem = new TestItemConfigParam();
        #endregion

        #region 良率信息
        public struct ItemYield
        {
            public double bBlackEN;
            public double bInfraredDarkEN;
            public double bBlemishEN;
            public double bBadPixelEN;
            public double bHotPixelEN;
            public double bWBEN;
            public double bMTFEN;
            public double bIRMTFEN;
            public double bShadingEN;
            public double bGrayEN;
            public double bColorEN;
            public double bFOVEN;
            public double bFPSEN;
            public double bAlignmentEN;
            public double bDistortionEN;
            public double bFWEN;
            public double bVoltageEN;
            public double bCurrentEN;
            public double bPowerEN;
            public double bSNREN;
            public double bRotationEN;
            public double bChangeViewEN;

        };

        public ItemYield ItemCount = new ItemYield();
        public ItemYield ItemCount1 = new ItemYield();

        public string[] ItemName = new string[]
        {
            "bBlackEN,暗板不良",
            "bInfraredDarkEN,暗场红外亮度不良",
            "bBlemishEN,脏污不良",
            "bBadPixelEN,坏点不良",
            "bHotPixelEN,亮点不良",
            "bWBEN,白平衡不良",
            "bMTFEN,清晰度不良",
            "bIRMTFEN,IR清晰度不良",
            "bShadingEN,Shading不良",
            "bGrayEN,灰阶不良",
            "bColorEN,色彩不良",
            "bFOVEN,视场角不良",
            "bFPSEN,帧率不良",
            "bAlignmentEN,光学中心不良",
            "bDistortionEN,畸变不良",
            "bFWEN,固件版本",
            "bVoltageEN,电压不良",
            "bCurrentEN,电流不良",
            "bPowerEN,功率不良",
            "bSNREN,SNR不良",
            "bRotationEN,旋转倾斜不良",
            "bChangeViewEN,切换不良",
        };
        #endregion

    }
}
