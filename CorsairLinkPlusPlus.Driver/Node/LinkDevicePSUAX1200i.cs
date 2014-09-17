using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class LinkDevicePSUAX1200i : LinkDevicePSU
    {
        internal LinkDevicePSUAX1200i(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        internal override int GetPCIeRailCount()
        {
            return 8;
        }
    }
}
