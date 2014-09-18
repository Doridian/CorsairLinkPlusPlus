using HidLibrary;
using System.Collections.Generic;
using System.Threading;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class RootDevice : Driver.BaseDevice
    {
        const int VID_CORSAIR_LINK = 0x1B1C;

        const int PID_CORSAIR_COMMANDER_LINK_A = 0x0C00;
        const int PID_CORSAIR_COMMANDER_LINK_B = 0x0C02;
        const int PID_CORSAIR_BOOTLOADER = 0x0C01;
        const int PID_CORSAIR_MODERN = 0x0C04;

        internal static readonly Mutex usbGlobalMutex = new Mutex(true, "Access_CorsairLink");

        private RootDevice() : base(null)
        {

        }

        private static readonly object instanceLock = new object();
        private volatile static RootDevice instance = null;

        public static RootDevice GetInstance()
        {
            lock (instanceLock)
            {
                if (instance == null)
                    instance = new RootDevice();
                return instance;
            }
        }

        public override string GetName()
        {
            return "Corsair Root Device";
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);

            if (volatileOnly)
                return;
        }

        internal override List<Driver.BaseDevice> GetSubDevicesInternal()
        {
            List<Driver.BaseDevice> ret = base.GetSubDevicesInternal();

            IEnumerable<HidDevice> hidDevices = HidDevices.Enumerate(VID_CORSAIR_LINK, new int[] {
                        PID_CORSAIR_COMMANDER_LINK_A,
                        PID_CORSAIR_COMMANDER_LINK_B,
                        PID_CORSAIR_BOOTLOADER,
                        PID_CORSAIR_MODERN
                    });

            foreach (HidDevice hidDevice in hidDevices)
            {
                USB.BaseUSBDevice device;
                switch (hidDevice.Attributes.ProductId)
                {
                    case PID_CORSAIR_COMMANDER_LINK_A:
                        device = new DeviceCommanderA(this, hidDevice);
                        break;
                    case PID_CORSAIR_COMMANDER_LINK_B:
                        device = new DeviceCommanderB(this, hidDevice);
                        break;
                    case PID_CORSAIR_BOOTLOADER:
                        device = new DeviceBootloader(this, hidDevice);
                        break;
                    case PID_CORSAIR_MODERN:
                        device = new DeviceModern(this, hidDevice);
                        break;
                    default:
                        device = null;
                        break;
                }
                if (device != null)
                    ret.Add(device);
            }

            return ret;
        }

        public override string GetLocalDeviceID()
        {
            return "CorsairRoot";
        }
    }
}
