using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Motion.Interfaces.Base;

namespace Motion.Interfaces
{
    public delegate void DataReceiveCompleteEventHandler(object sender,string result);

    public struct TriggerArgs
    {
        public object sender;
        public string cmd;
        public byte tryTimes;
        public string message;
    }

    public interface ITriggerModel : IDevice
    {
        event DataReceiveCompleteEventHandler DeviceDataReceiveComplete;

        IAsyncResult BeginTrigger(TriggerArgs args);
        string Execute(string cmd);
        void Trigger(TriggerArgs args);
        string StopTrigger();
    }

    public interface ISerialPortTriggerModel : ISerialPort, ITriggerModel { }
    public interface INetworkTriggerModel : INetWork, ITriggerModel { }
       
}
