using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class CorsairLinkDeviceAFP : CorsairLinkDevice
    {
        internal CorsairLinkDeviceAFP(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }
        public override string GetName()
        {
            return "Corsair AirFlow Pro";
        }

        public override List<CorsairBaseDevice> GetSubDevices()
        {
            List<CorsairBaseDevice> ramSticks = base.GetSubDevices();
            for (int i = 0; i < 6; i++)
            {
                CorsairLinkAFPRAMStick ramStick = new CorsairLinkAFPRAMStick(this, channel, i);
                if(ramStick.IsPresent())
                    ramSticks.Add(ramStick);
            }
            return ramSticks;
        }
    }
    public class CorsairLinkAFPRAMStick : CorsairLinkDevice
    {
        protected readonly int id;
        protected readonly CorsairLinkDeviceAFP afpDevice;

        internal CorsairLinkAFPRAMStick(CorsairLinkDeviceAFP device, byte channel, int id)
            : base(device.usbDevice, channel)
        {
            this.id = id;
            this.afpDevice = device;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            this.afpDevice.Refresh(volatileOnly);
        }

        public override string GetUDID()
        {
            return base.GetUDID() + "/DIMM" + id;
        }

        public override string GetName()
        {
            return "Corsair AirFlow Pro RAM stick " + id;
        }

        internal byte[] GetReadings()
        {
            return ReadRegister((byte)(id << 4), 16);
        }

        public new bool IsPresent()
        {
            return GetReadings()[0] != 0;
        }

        public override List<CorsairSensor> GetSensors()
        {
            List<CorsairSensor> ret = base.GetSensors();
            ret.Add(new CorsairAFPThermistor(this, 0));
            ret.Add(new CorsairAFPUsage(this, 0));
            return ret;
        }

        public class CorsairAFPThermistor : CorsairUsage
        {
            private readonly CorsairLinkAFPRAMStick afpDevice;
            internal CorsairAFPThermistor(CorsairLinkAFPRAMStick device, int id)
                : base(device, id)
            {
                this.afpDevice = device;
            }

            internal override double GetValueInternal()
            {
                return 1;
            }

            public override string GetSensorType()
            {
                return "Memory usage";
            }
        }

        public class CorsairAFPUsage : CorsairUsage
        {
            private readonly CorsairLinkAFPRAMStick afpDevice;
            internal CorsairAFPUsage(CorsairLinkAFPRAMStick device, int id)
                : base(device, id)
            {
                this.afpDevice = device;
            }

            internal override double GetValueInternal()
            {
                return afpDevice.GetReadings()[2];
            }

            public override string GetSensorType()
            {
                return "Memory usage";
            }
        }
    }
}
