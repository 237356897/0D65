using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motion.Interfaces;
namespace Motion.Enginee
{
    public class External : IExternal
    {
        public bool AirSignal { get; set; }

        public bool AlarmReset { get; set; }

        public bool AutoMode { get; set; }

        public bool InitializingDone { get; set; }

        public bool ManualMode { get; set; }
    }
}
