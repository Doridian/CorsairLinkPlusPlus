using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Link
{
    public class CorsairLinkDeviceAXPSU : CorsairLinkDevice
    {
        internal CorsairLinkDeviceAXPSU(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }
        public override string GetDeviceName()
        {
            byte[] ret = ReadRegister(0x9A, 7);
            return System.Text.Encoding.UTF8.GetString(ret);
        }
    }
}
