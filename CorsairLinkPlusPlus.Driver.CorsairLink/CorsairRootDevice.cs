#region LICENSE
/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
 #endregion

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

        public override DeviceType Type
        {
            get
            {
                return DeviceType.Root;
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
