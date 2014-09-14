using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkDeviceEnumerator
    {
        public List<CorsairLinkUSBDevice> GetDevices()
        {
            IEnumerable<HidDevice> hidDevices = HidDevices.Enumerate(0x1B1C /* Corsair VID */);

            List<CorsairLinkUSBDevice> devices = new List<CorsairLinkUSBDevice>();
            foreach (HidDevice hidDevice in hidDevices)
            {
                CorsairLinkUSBDevice device = CorsairLinkUSBDevice.CreateFromHIDDevice(hidDevice);
                if(device != null)
                    devices.Add(device);
            }
            return devices;
        }
    }
}
