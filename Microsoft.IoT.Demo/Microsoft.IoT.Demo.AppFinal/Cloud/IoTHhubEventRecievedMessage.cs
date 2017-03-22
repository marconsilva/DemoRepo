using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.IoT.Demo.AppFinal.Cloud
{
    public class IoTHhubEventRecievedMessage : EventArgs
    {
        public IDictionary<string,string> Properties { get; set; }
    }
}
