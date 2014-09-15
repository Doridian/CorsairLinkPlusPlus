using HidLibrary;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkDeviceEnumerator
    {
        const int VID_CORSAIR_LINK = 0x1B1C;

        const int PID_CORSAIR_COMMANDER_LINK_A = 0x0C00;
        const int PID_CORSAIR_COMMANDER_LINK_B = 0x0C02;
        const int PID_CORSAIR_BOOTLOADER = 0x0C01;
        const int PID_CORSAIR_MODERN = 0x0C04;

        public List<CorsairLinkUSBDevice> GetDevices()
        {
            IEnumerable<HidDevice> hidDevices = HidDevices.Enumerate(VID_CORSAIR_LINK, new int[] {
                PID_CORSAIR_COMMANDER_LINK_A,
                PID_CORSAIR_COMMANDER_LINK_B,
                PID_CORSAIR_BOOTLOADER,
                PID_CORSAIR_MODERN
            });

            List<CorsairLinkUSBDevice> devices = new List<CorsairLinkUSBDevice>();
            foreach (HidDevice hidDevice in hidDevices)
            {
                CorsairLinkUSBDevice device;
                switch (hidDevice.Attributes.ProductId)
                {
                    case PID_CORSAIR_COMMANDER_LINK_A:
                        device = new CorsairLinkUSBDeviceCommanderA(hidDevice);
                        break;
                    case PID_CORSAIR_COMMANDER_LINK_B:
                        device = new CorsairLinkUSBDeviceCommanderB(hidDevice);
                        break;
                    case PID_CORSAIR_BOOTLOADER:
                        device = new CorsairLinkUSBDeviceBootloader(hidDevice);
                        break;
                    case PID_CORSAIR_MODERN:
                        device = new CorsairLinkUSBDeviceModern(hidDevice);
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
