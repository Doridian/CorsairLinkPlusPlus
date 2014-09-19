using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.CorsairLink.Registry;
using HidLibrary;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
    public class CorsairRootDevice : BaseDevice, IRootDevice, IDisposable
    {
        const int VID_CORSAIR_LINK = 0x1B1C;

        const int PID_CORSAIR_COMMANDER_LINK_A = 0x0C00;
        const int PID_CORSAIR_COMMANDER_LINK_B = 0x0C02;
        const int PID_CORSAIR_BOOTLOADER = 0x0C01;
        const int PID_CORSAIR_MODERN = 0x0C04;

        internal static readonly Mutex usbGlobalMutex = new Mutex(false, "Global\\Access_CorsairLink");

        public CorsairRootDevice()
            : base(RootDevice.GetInstance())
        {
            usbGlobalMutex.WaitOne();
            FanControllerRegistry.Initialize();
            LEDControllerRegistry.Initialize();
        }

        public void Dispose()
        {
            usbGlobalMutex.ReleaseMutex();
        }

        public override string GetName()
        {
            return "Corsair Link";
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);

            if (volatileOnly)
                return;
        }

        protected override List<IDevice> GetSubDevicesInternal()
        {
            List<IDevice> ret = base.GetSubDevicesInternal();

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
            return "CorsairLink";
        }
    }
}
