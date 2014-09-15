using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Link
{
    public class CorsairLinkDeviceAFP : CorsairLinkDevice
    {
        internal CorsairLinkDeviceAFP(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }
        public override string GetDeviceName()
        {
            return "AirFlow Pro";
        }
    }
}
