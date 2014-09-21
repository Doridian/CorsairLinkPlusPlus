using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.CorsairLink.Registry;
using HidSharp;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using CorsairLinkPlusPlus.Common.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
    public class CorsairRootDevice : BaseDevice, IRootDevice
    {
        const int VID_CORSAIR_LINK = 0x1B1C;

        const int PID_CORSAIR_COMMANDER_LINK_A = 0x0C00;
        const int PID_CORSAIR_COMMANDER_LINK_B = 0x0C02;
        const int PID_CORSAIR_BOOTLOADER = 0x0C01;
        const int PID_CORSAIR_MODERN = 0x0C04;

        internal static readonly DisposableMutex usbGlobalMutex = new DisposableMutex("Global\\Access_CorsairLink");

        private void AssertConflicts()
        {
            Process[] linkProcesses = Process.GetProcessesByName("CorsairLINK");
            foreach (Process process in linkProcesses)
            {
                FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(process.Modules[0].FileName);
                if (fileVersion.ProductName == "CorsairLINK")
                    throw new LinkAccessException("CorsairLINK is already running");
                 
            }
        }

        public CorsairRootDevice()
            : base(RootDevice.GetInstance())
        {
            AssertConflicts();
            FanControllerRegistry.Initialize();
            LEDControllerRegistry.Initialize();
        }

        public override string Name
        {
            get
            {
                return "Corsair Link";
            }
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

            IEnumerable<HidDevice> hidDevices = new HidDeviceLoader().GetDevices(VID_CORSAIR_LINK);

            foreach (HidDevice hidDevice in hidDevices)
            {
                USB.BaseUSBDevice device;
                switch (hidDevice.ProductID)
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
