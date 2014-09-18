using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Node.Internal;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.Sensor.Internal;
using CorsairLinkPlusPlus.Driver.USB;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class LinkDevicePSU : BaseLinkDevice
    {
        internal static LinkDevicePSU CreateNew(USB.BaseUSBDevice usbDevice, byte channel)
        {
            LinkDevicePSU genericPSU = new LinkDevicePSU(usbDevice, channel);
            string psuName = genericPSU.GetInternalName();
            if (psuName == "AX1200i")
                return new LinkDevicePSUAX1200i(usbDevice, channel);
            if (psuName == "AX1500i")
                return new LinkDevicePSUAX1500i(usbDevice, channel);
            if (psuName.StartsWith("HX") && psuName != "HX1200i" && psuName != "HX1000i")
                return new LinkDevicePSUHXiNoRail(usbDevice, channel);
            return genericPSU;
        }

        internal virtual string[] GetSecondary12VRailNames()
        {
            return new string[]
            {
			    "PCIe 1",
			    "PCIe 2",
			    "PCIe 3",
			    "PCIe 4",
			    "PCIe 5",
			    "PCIe 6",
			    "PCIe 7",
			    "PCIe 8",
			    "PSU 12V",
			    "PERIPHERAL 12V"
		    };
        }

        internal virtual int GetPCIeRailCount()
        {
            return 6;
        }

        internal virtual string[] GetMainRailNames()
        {
            return new string[]
            {
			    "PSU 5V",
			    "PSU 3.3V"
		    };
        }

        internal LinkDevicePSU(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        internal string GetInternalName()
        {
            byte[] ret = ReadRegister(0x9A, 7);
            return System.Text.Encoding.UTF8.GetString(ret);
        }

        public override string GetName()
        {
            return "Corsair PSU " + GetInternalName();
        }

        internal void SetMainPage(int page)
        {
            DisabledCheck();

            WriteSingleByteRegister(0x00, (byte)page);
        }

        private void SetSecondary12VPage(int page)
        {
            WriteSingleByteRegister(0xE7, (byte)page);
        }

        internal double GetSecondary12VCurrent(int page)
        {
            DisabledCheck();

            byte[] ret;
            
            RootDevice.usbGlobalMutex.WaitOne();
            SetMainPage(0);
            SetSecondary12VPage(page);
            ret = ReadRegister(0xE8, 2);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return BitCodec.ToFloat(ret);
        }

        class Secondary12VCurrentSensor : CurrentSensor
        {
            private readonly string name;
            private readonly LinkDevicePSU psuDevice;

            internal Secondary12VCurrentSensor(LinkDevicePSU device, int id, string name)
                : base(device, id)
            {
                this.name = name;
                this.psuDevice = device;
            }

            protected override double GetValueInternal()
            {
                DisabledCheck();
                return psuDevice.GetSecondary12VCurrent(id);
            }

            public override string GetName()
            {
                return name + " Current";
            }
        }

        protected override List<BaseDevice> GetSubDevicesInternal()
        {
            List<BaseDevice> ret = base.GetSubDevicesInternal();

            ret.Add(new ThermistorPSU(this, 0));
            ret.Add(new FanPSU(this, 0));

            string[] mainRailNames = GetMainRailNames();
            for (int i = 0; i < mainRailNames.Length; i++)
                ret.Add(new PSUMainPowerDevice(this, channel, i + 1, mainRailNames[i]));

            string[] secondary12VRails = GetSecondary12VRailNames();
            if (secondary12VRails.Length > 0)
            {
                for (int i = 0; i < GetPCIeRailCount(); i++)
                {
                    ret.Add(new Secondary12VCurrentSensor(this, i, secondary12VRails[i]));
                }

                ret.Add(new Secondary12VCurrentSensor(this, secondary12VRails.Length - 2, secondary12VRails[secondary12VRails.Length - 2]));
                ret.Add(new Secondary12VCurrentSensor(this, secondary12VRails.Length - 1, secondary12VRails[secondary12VRails.Length - 1]));
            }

            return ret;
        }
    }
}
