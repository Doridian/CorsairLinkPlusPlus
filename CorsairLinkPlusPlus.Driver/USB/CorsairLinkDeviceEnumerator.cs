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
        const int VID_CORSAIR_LINK = 0x1B1C;

        const int PID_CORSAIR_LINK_OLD = 0x0C02;
        const int PID_CORSAIR_LINK_NEW = 0x0C04;

        public List<CorsairLinkUSBDevice> GetDevices()
        {
            IEnumerable<HidDevice> hidDevices = HidDevices.Enumerate(VID_CORSAIR_LINK, new int[] { PID_CORSAIR_LINK_NEW, PID_CORSAIR_LINK_OLD });

            List<CorsairLinkUSBDevice> devices = new List<CorsairLinkUSBDevice>();
            foreach (HidDevice hidDevice in hidDevices)
            {
                CorsairLinkUSBDevice device;
                switch (hidDevice.Attributes.ProductId)
                {
                    case PID_CORSAIR_LINK_OLD:
                        device = new CorsairLinkUSBDeviceOld(hidDevice);
                        break;
                    case PID_CORSAIR_LINK_NEW:
                        device = new CorsairLinkUSBDevice(hidDevice);
                        break;
                    default:
                        device = null;
                        break;
                }
                if(device != null)
                    devices.Add(device);
            }
            return devices;
        }
    }
}
