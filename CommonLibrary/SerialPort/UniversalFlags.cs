using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public delegate void DataReceiveCompleteEventHandler(object sender, string result);

    public static class UniversalFlags
    {
        #region 标志

        public const string successStr = "Success! ";
        //public const string failStr = "Fail! ";
        public const string errorStr = "Error! ";

        #endregion
    }
}
