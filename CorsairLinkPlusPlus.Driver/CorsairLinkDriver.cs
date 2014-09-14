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
        public List<CorsairLinkDevice> GetDevices()
        {
            IEnumerable<HidDevice> hidDevices = HidDevices.Enumerate(0x1B1C /* Corsair VID */);

            List<CorsairLinkDevice> devices = new List<CorsairLinkDevice>();
            foreach (HidDevice hidDevice in hidDevices)
            {
                CorsairLinkDevice device = CorsairLinkDevice.CreateFromHIDDevice(hidDevice);
                if(device != null)
                    devices.Add(device);
            }
            return devices;
        }
    }
}
