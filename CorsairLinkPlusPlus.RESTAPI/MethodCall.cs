using CorsairLinkPlusPlus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.RESTAPI
{
    public class MethodCall
    {
        public string Name;
        public Dictionary<string, object> Params;
        public IDevice Device;

        public MethodCall()
        {

        }

        public void Execute()
        {
            Methods.BaseMethod method;
            switch (Name)
            {
                case "Refresh":
                    method = new Methods.Refresh();
                    break;
                case "SetController":
                    method = new Methods.SetController();
                    break;
                default:
                    throw new ArgumentException("Invalid method");
            }
            method.Arguments = Params;
            method.Device = Device;
            if(!method.IsDeviceValid())
                throw new ArgumentException("Invalid method for device");
            method.Call();
        }
    }
}
