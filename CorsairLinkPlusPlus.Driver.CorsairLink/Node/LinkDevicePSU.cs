using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node.Internal;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Node
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
            if (psuName.StartsWith("HX"))
                return new LinkDevicePSUHX(usbDevice, channel);
            return genericPSU;
        }

        internal string internalNameCache = null;

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

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            if (!volatileOnly)
                internalNameCache = null;
        }

        internal LinkDevicePSU(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        internal string GetInternalName()
        {
            if(internalNameCache == null)
            {
                byte[] ret = ReadRegister(0x9A, 7);
                internalNameCache = System.Text.Encoding.UTF8.GetString(ret);
            }

            return internalNameCache;
        }

        public override string Name
        {
            get
            {
                return "Corsair PSU " + GetInternalName();
            }
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

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                SetMainPage(0);
                SetSecondary12VPage(page);
                ret = ReadRegister(0xE8, 2);
            }

            return BitCodec.ToFloat(ret);
        }

        protected override List<IDevice> GetSubDevicesInternal()
        {
            List<IDevice> ret = base.GetSubDevicesInternal();

            ret.Add(new ThermistorPSU(this, 0));
            ret.Add(new FanPSU(this, 0));

            string[] mainRailNames = GetMainRailNames();
            for (int i = 0; i < mainRailNames.Length; i++)
                ret.Add(new PSUPrimaryPowerDevice(this, channel, i + 1, mainRailNames[i]));

            string[] secondary12VRails = GetSecondary12VRailNames();
            if (secondary12VRails.Length > 0)
            {
                for (int i = 0; i < GetPCIeRailCount(); i++)
                    ret.Add(new Secondary12VCurrentSensor(this, i, secondary12VRails[i]));

                ret.Add(new Secondary12VCurrentSensor(this, secondary12VRails.Length - 2, secondary12VRails[secondary12VRails.Length - 2]));
                ret.Add(new Secondary12VCurrentSensor(this, secondary12VRails.Length - 1, secondary12VRails[secondary12VRails.Length - 1]));
            }

            ret.Add(new PSUMainsPowerDevice(this, channel, "Mains"));

            return ret;
        }
    }
}
