using HidLibrary;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairRootDevice : CorsairBaseDevice
    {
        const int VID_CORSAIR_LINK = 0x1B1C;

        const int PID_CORSAIR_COMMANDER_LINK_A = 0x0C00;
        const int PID_CORSAIR_COMMANDER_LINK_B = 0x0C02;
        const int PID_CORSAIR_BOOTLOADER = 0x0C01;
        const int PID_CORSAIR_MODERN = 0x0C04;

        private CorsairRootDevice()
        {

        }

        private static CorsairRootDevice instance = null;

        public static CorsairRootDevice GetInstance()
        {
            if (instance == null)
                instance = new CorsairRootDevice();
            return instance;
        }

        public bool IsPresent()
        {
            return true;
        }

        public string GetName()
        {
            return "Corsair Root Device";
        }

        public void Refresh(bool volatileOnly)
        {
            if (volatileOnly)
                return;

            lock (instance)
            {
                foreach (CorsairLinkUSBDevice usbDevice in devices)
                {
                    usbDevice.disabled = true;
                }

                devices = null;
            }
        }

        private List<CorsairLinkUSBDevice> devices = null;

        public List<CorsairBaseDevice> GetSubDevices()
        {
            List<CorsairBaseDevice> ret = new List<CorsairBaseDevice>();

            lock (instance)
            {
                if (devices == null)
                {
                    IEnumerable<HidDevice> hidDevices = HidDevices.Enumerate(VID_CORSAIR_LINK, new int[] {
                    PID_CORSAIR_COMMANDER_LINK_A,
                    PID_CORSAIR_COMMANDER_LINK_B,
                    PID_CORSAIR_BOOTLOADER,
                    PID_CORSAIR_MODERN
                });

                    devices = new List<CorsairLinkUSBDevice>();
                    foreach (HidDevice hidDevice in hidDevices)
                    {
                        CorsairLinkUSBDevice device;
                        switch (hidDevice.Attributes.ProductId)
                        {
                            case PID_CORSAIR_COMMANDER_LINK_A:
                                device = new CorsairLinkUSBDeviceCommanderA(this, hidDevice);
                                break;
                            case PID_CORSAIR_COMMANDER_LINK_B:
                                device = new CorsairLinkUSBDeviceCommanderB(this, hidDevice);
                                break;
                            case PID_CORSAIR_BOOTLOADER:
                                device = new CorsairLinkUSBDeviceBootloader(this, hidDevice);
                                break;
                            case PID_CORSAIR_MODERN:
                                device = new CorsairLinkUSBDeviceModern(this, hidDevice);
                                break;
                            default:
                                device = null;
                                break;
                        }
                        if (device != null)
                            devices.Add(device);
                    }
                }

                ret.AddRange(devices);
            }

            return ret;
        }

        public string GetUDID()
        {
            return "/";
        }

        public CorsairBaseDevice GetParent()
        {
            return null;
        }
    }
}
