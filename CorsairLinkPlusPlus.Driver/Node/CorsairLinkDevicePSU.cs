using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class CorsairLinkDevicePSUHXiNoRail : CorsairLinkDevicePSU
    {
        internal CorsairLinkDevicePSUHXiNoRail(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }
        internal override string[] GetSecondary12VRailNames()
        {
            return new string[0];
        }

        internal override int GetPCIeRailCount()
        {
            return 0;
        }
    }

    public class CorsairLinkDevicePSUAX1500i : CorsairLinkDevicePSU
    {
        internal CorsairLinkDevicePSUAX1500i(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        internal override string[] GetSecondary12VRailNames()
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
                "PCIe 9",
			    "PCIe 10",
			    "PSU 12V",
			    "PERIPHERAL 12V"
		    };
        }

        internal override int GetPCIeRailCount()
        {
            return 10;
        }
    }
    public class CorsairLinkDevicePSUAX1200i : CorsairLinkDevicePSU
    {
        internal CorsairLinkDevicePSUAX1200i(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        internal override int GetPCIeRailCount()
        {
            return 8;
        }
    }

    public class CorsairLinkDevicePSU : CorsairLinkDevice
    {
        internal static CorsairLinkDevicePSU CreateNew(CorsairLinkUSBDevice usbDevice, byte channel)
        {
            CorsairLinkDevicePSU genericPSU = new CorsairLinkDevicePSU(usbDevice, channel);
            string psuName = genericPSU.GetInternalName();
            if (psuName == "AX1200i")
                return new CorsairLinkDevicePSUAX1200i(usbDevice, channel);
            if (psuName == "AX1500i")
                return new CorsairLinkDevicePSUAX1500i(usbDevice, channel);
            if (psuName.StartsWith("HX") && psuName != "HX1200i" && psuName != "HX1000i")
                return new CorsairLinkDevicePSUHXiNoRail(usbDevice, channel);
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

        internal CorsairLinkDevicePSU(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        internal string GetInternalName()
        {
            byte[] ret = ReadRegister(0x9A, 7);
            return System.Text.Encoding.UTF8.GetString(ret);
        }

        public override string GetName()
        {
            return "Corsair PSU " + GetInternalName();
        }

        class CorsairThermistorPSU : CorsairThermistor
        {
            internal CorsairThermistorPSU(CorsairLinkDevicePSU device, int id)
                : base(device, id)
            {

            }

            internal override double GetValueInternal()
            {
                return CorsairBitCodec.ToFloat(device.ReadRegister(0x8E, 2), 0);
            }
        }
        class CorsairFanPSU : CorsairFan
        {
            internal CorsairFanPSU(CorsairLinkDevicePSU device, int id)
                : base(device, id)
            {

            }

            internal override double GetValueInternal()
            {
                return CorsairBitCodec.ToFloat(device.ReadRegister(0x90, 2), 0);
            }
        }

        internal void SetMainPage(int page)
        {
            WriteSingleByteRegister(0x00, (byte)page);
        }

        internal void SetSecondary12VPage(int page)
        {
            WriteSingleByteRegister(0xE7, (byte)page);
        }

        internal double GetSecondary12VCurrent(int page)
        {
            byte[] ret;
            lock (usbDevice.usbLock)
            {
                SetMainPage(0);
                SetSecondary12VPage(page);
                ret = ReadRegister(0xE8, 2);
            }
            return CorsairBitCodec.ToFloat(ret);
        }

        class CorsairSecondary12VCurrentSensor : CorsairCurrentSensor
        {
            private readonly string name;
            private readonly CorsairLinkDevicePSU psuDevice;

            internal CorsairSecondary12VCurrentSensor(CorsairLinkDevicePSU device, int id, string name)
                : base(device, id)
            {
                this.name = name;
                this.psuDevice = device;
            }

            internal override double GetValueInternal()
            {
                return psuDevice.GetSecondary12VCurrent(id);
            }

            public override string GetName()
            {
                return name + " Current";
            }
        }

        class CorsairMainPowerDevice : CorsairLinkDevice
        {
            protected readonly int id;
            protected readonly string name;
            protected readonly CorsairLinkDevicePSU psuDevice;
            internal CorsairMainPowerDevice(CorsairLinkDevicePSU psuDevice, byte channel, int id, string name)
                : base(psuDevice.usbDevice, channel)
            {
                this.id = id;
                this.name = name;
                this.psuDevice = psuDevice;
            }

            public override void Refresh(bool volatileOnly)
            {
                base.Refresh(volatileOnly);
                this.psuDevice.Refresh(volatileOnly);
            }

            public override string GetUDID()
            {
                return psuDevice.GetUDID() + "/Main" + id;
            }

            class CorsairMainPowerSensor : CorsairPowerSensor
            {
                private readonly CorsairMainPowerDevice powerDevice;

                internal CorsairMainPowerSensor(CorsairMainPowerDevice device)
                    : base(device, 0)
                {
                    this.powerDevice = device;
                }

                internal override double GetValueInternal()
                {
                    return powerDevice.ReadPower();
                }
            }

            class CorsairMainCurrentSensor : CorsairCurrentSensor
            {
                private readonly CorsairMainPowerDevice powerDevice;

                internal CorsairMainCurrentSensor(CorsairMainPowerDevice device)
                    : base(device, 0)
                {
                    this.powerDevice = device;
                }

                internal override double GetValueInternal()
                {
                    return powerDevice.ReadCurrent();
                }
            }

            class CorsairMainVoltageSensor : CorsairVoltageSensor
            {
                private readonly CorsairMainPowerDevice powerDevice;

                internal CorsairMainVoltageSensor(CorsairMainPowerDevice device)
                    : base(device, 0)
                {
                    this.powerDevice = device;
                }

                internal override double GetValueInternal()
                {
                    return powerDevice.ReadVoltage();
                }
            }

            public override string GetName()
            {
                return name;
            }

            public override List<CorsairSensor> GetSensors()
            {
                List<CorsairSensor> sensors = base.GetSensors();
                sensors.Add(new CorsairMainPowerSensor(this));
                sensors.Add(new CorsairMainCurrentSensor(this));
                sensors.Add(new CorsairMainVoltageSensor(this));
                return sensors;
            }

            internal void SetPage()
            {
                psuDevice.SetMainPage(id);
            }

            internal double ReadVoltage()
            {
                byte[] ret;
                lock (usbDevice.usbLock)
                {
                    SetPage();
                    ret = ReadRegister(0x8B, 2);
                }
                return CorsairBitCodec.ToFloat(ret);
            }

            internal double ReadCurrent()
            {
                byte[] ret;
                lock (usbDevice.usbLock)
                {
                    SetPage();
                    ret = ReadRegister(0x8C, 2);
                }
                return CorsairBitCodec.ToFloat(ret);
            }

            internal double ReadPower()
            {
                byte[] ret;
                lock (usbDevice.usbLock)
                {
                    SetPage();
                    ret = ReadRegister(0x96, 2);
                }
                return CorsairBitCodec.ToFloat(ret);
            }
        }

        public override List<CorsairSensor> GetSensors()
        {
            List<CorsairSensor> ret = base.GetSensors();
            ret.Add(new CorsairThermistorPSU(this, 0));
            ret.Add(new CorsairFanPSU(this, 0));

            string[] secondary12VRails = GetSecondary12VRailNames();
            if (secondary12VRails.Length > 0)
            {
                for (int i = 0; i < GetPCIeRailCount(); i++)
                {
                    ret.Add(new CorsairSecondary12VCurrentSensor(this, i, secondary12VRails[i]));
                }

                ret.Add(new CorsairSecondary12VCurrentSensor(this, secondary12VRails.Length - 2, secondary12VRails[secondary12VRails.Length - 2]));
                ret.Add(new CorsairSecondary12VCurrentSensor(this, secondary12VRails.Length - 1, secondary12VRails[secondary12VRails.Length - 1]));
            }
            return ret;
        }

        public override List<CorsairBaseDevice> GetSubDevices()
        {
            List<CorsairBaseDevice> ret = base.GetSubDevices();
            string[] mainRailNames = GetMainRailNames();
            for (int i = 0; i < mainRailNames.Length; i++)
            {
                CorsairMainPowerDevice psuSubSensor = new CorsairMainPowerDevice(this, channel, i + 1, mainRailNames[i]);
                if (psuSubSensor.IsPresent())
                    ret.Add(psuSubSensor);
            }
            return ret;
        }
    }
}
