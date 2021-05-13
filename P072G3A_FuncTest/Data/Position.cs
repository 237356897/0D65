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
            public bool bAllTestEN;
            public bool bSNREN;
            public bool bRotationEN;
            public bool bChangeViewEN;

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

        #region 位置信息



        #endregion

    }
}
