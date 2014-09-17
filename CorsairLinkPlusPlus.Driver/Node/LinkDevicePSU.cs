using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class LinkDevicePSUHXiNoRail : LinkDevicePSU
    {
        internal LinkDevicePSUHXiNoRail(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }
        internal override string[] GetSecondary12VRailNames()
        {
            return new string[0];
        }

        internal override int GetPCIeRailCount()
        {
            return 0;
        }
    }

    public class LinkDevicePSUAX1500i : LinkDevicePSU
    {
        internal LinkDevicePSUAX1500i(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

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
    public class LinkDevicePSUAX1200i : LinkDevicePSU
    {
        internal LinkDevicePSUAX1200i(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        internal override int GetPCIeRailCount()
        {
            return 8;
        }
    }

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

        class ThermistorPSU : Thermistor
        {
            internal ThermistorPSU(LinkDevicePSU device, int id)
                : base(device, id)
            {

            }

            internal override double GetValueInternal()
            {
                return BitCodec.ToFloat(device.ReadRegister(0x8E, 2), 0);
            }
        }
        class FanPSU : Fan, ControllableSensor
        {
            private ControllerBase controller = null;

            internal FanPSU(LinkDevicePSU device, int id)
                : base(device, id)
            {

            }

            internal override double GetValueInternal()
            {
                DisabledCheck();

                return BitCodec.ToFloat(device.ReadRegister(0x90, 2), 0);
            }

            public override void SetFixedPercent(int percent)
            {
                DisabledCheck();

                if (percent < 0 || percent > 100)
                    throw new ArgumentException();
                device.WriteSingleByteRegister(0x3B, (byte)percent);
            }

            public override int GetFixedPercent()
            {
                DisabledCheck();

                return device.ReadSingleByteRegister(0x3B);
            }

            public void SetController(ControllerBase controller)
            {
                DisabledCheck();

                if (controller is FanDefaultController)
                    device.WriteSingleByteRegister(0xF0, 0);
                else if (controller is FanFixedPercentController)
                    device.WriteSingleByteRegister(0xF0, 1);

                SaveControllerData(controller);
            }

            public void SaveControllerData(ControllerBase controller)
            {
                DisabledCheck();

                controller.Apply(this);
            }

            public ControllerBase GetController()
            {
                DisabledCheck();

                if (controller == null)
                    switch(device.ReadSingleByteRegister(0xF0))
                    {
                        case 0:
                            controller = new FanDefaultController();
                            break;
                        case 1:
                            FanFixedPercentController newController = new FanFixedPercentController();
                            newController.AssignFrom(this);
                            controller = newController;
                            break;
                    }
                    
                return controller;
            }

            public override int GetFixedRPM()
            {
                throw new NotImplementedException();
            }

            public override void SetFixedRPM(int rpm)
            {
                throw new NotImplementedException();
            }

            public override ControlCurve GetControlCurve()
            {
                throw new NotImplementedException();
            }

            public override void SetControlCurve(ControlCurve curve)
            {
                throw new NotImplementedException();
            }

            public override void SetMinimalRPM(int rpm)
            {
                throw new NotImplementedException();
            }

            public override int GetMinimalRPM()
            {
                DisabledCheck();

                return 0;
            }
        }

        private void SetMainPage(int page)
        {
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
            lock (usbDevice.usbLock)
            {
                SetMainPage(0);
                SetSecondary12VPage(page);
                ret = ReadRegister(0xE8, 2);
            }
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

            internal override double GetValueInternal()
            {
                DisabledCheck();
                return psuDevice.GetSecondary12VCurrent(id);
            }

            public override string GetName()
            {
                return name + " Current";
            }
        }

        class MainPowerDevice : BaseLinkDevice
        {
            protected readonly int id;
            protected readonly string name;
            protected readonly LinkDevicePSU psuDevice;
            internal MainPowerDevice(LinkDevicePSU psuDevice, byte channel, int id, string name)
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

            public override string GetLocalDeviceID()
            {
                return "PowerMain" + id;
            }

            class MainPowerSensor : PowerSensor
            {
                private readonly MainPowerDevice powerDevice;

                internal MainPowerSensor(MainPowerDevice device)
                    : base(device, 0)
                {
                    this.powerDevice = device;
                }

                internal override double GetValueInternal()
                {
                    DisabledCheck();
                    return powerDevice.ReadPower();
                }
            }

            class MainCurrentSensor : CurrentSensor
            {
                private readonly MainPowerDevice powerDevice;

                internal MainCurrentSensor(MainPowerDevice device)
                    : base(device, 0)
                {
                    this.powerDevice = device;
                }

                internal override double GetValueInternal()
                {
                    DisabledCheck();
                    return powerDevice.ReadCurrent();
                }
            }

            class MainVoltageSensor : VoltageSensor
            {
                private readonly MainPowerDevice powerDevice;

                internal MainVoltageSensor(MainPowerDevice device)
                    : base(device, 0)
                {
                    this.powerDevice = device;
                }

                internal override double GetValueInternal()
                {
                    DisabledCheck();
                    return powerDevice.ReadVoltage();
                }
            }

            public override string GetName()
            {
                return name;
            }

            internal override List<BaseDevice> GetSubDevicesInternal()
            {
                List<BaseDevice> sensors = base.GetSubDevicesInternal();
                sensors.Add(new MainCurrentSensor(this));
                sensors.Add(new MainPowerSensor(this));
                sensors.Add(new MainVoltageSensor(this));
                return sensors;
            }

            private void SetPage()
            {
                psuDevice.SetMainPage(id);
            }

            internal double ReadVoltage()
            {
                DisabledCheck();
                byte[] ret;
                lock (usbDevice.usbLock)
                {
                    SetPage();
                    ret = ReadRegister(0x8B, 2);
                }
                return BitCodec.ToFloat(ret);
            }

            internal double ReadCurrent()
            {
                DisabledCheck();
                byte[] ret;
                lock (usbDevice.usbLock)
                {
                    SetPage();
                    ret = ReadRegister(0x8C, 2);
                }
                return BitCodec.ToFloat(ret);
            }

            internal double ReadPower()
            {
                DisabledCheck();
                byte[] ret;
                lock (usbDevice.usbLock)
                {
                    SetPage();
                    ret = ReadRegister(0x96, 2);
                }
                return BitCodec.ToFloat(ret);
            }
        }

        internal override List<BaseDevice> GetSubDevicesInternal()
        {
            List<BaseDevice> ret = base.GetSubDevicesInternal();

            ret.Add(new ThermistorPSU(this, 0));
            ret.Add(new FanPSU(this, 0));

            string[] mainRailNames = GetMainRailNames();
            for (int i = 0; i < mainRailNames.Length; i++)
                ret.Add(new MainPowerDevice(this, channel, i + 1, mainRailNames[i]));

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
