using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class DatalogicBarScan : Serialport
    {
        public DatalogicBarScan(SerialPort serialPort) 
            : base(serialPort)
        {
            Port.NewLine = "\r\n";
        }

        private string trg;


        protected override string cmd { get { return trg; } set { trg = value; } }
        protected override void initDevice()
        {
            
        }

        protected override string triggerReadOnce()
        {
            string retDT = "TimeOut";
            Port.DiscardInBuffer();
            Port.Write(trg + "\r\n");
            string readDT = Port.ReadLine();
            retDT = readDT.Replace("\r\n", "");
            return retDT;
        }
    }
}
