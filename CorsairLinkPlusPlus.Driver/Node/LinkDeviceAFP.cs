using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class LinkDeviceAFP : BaseLinkDevice
    {
        internal LinkDeviceAFP(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }
        public override string GetName()
        {
            return "Corsair AirFlow Pro";
        }

        private List<LinkAFPRAMStick> ramSticks = null;

        public override List<BaseDevice> GetSubDevices()
        {
            List<BaseDevice> ret = base.GetSubDevices();
            if (ramSticks == null)
            {
                ramSticks = new List<LinkAFPRAMStick>();
                for (int i = 0; i < 6; i++)
                    ramSticks.Add(new LinkAFPRAMStick(this, channel, i));
            }

            foreach (LinkAFPRAMStick ramStick in ramSticks)
                if (ramStick.IsPresent())
                    ret.Add(ramStick);

            return ret;
        }
    }
    public class LinkAFPRAMStick : BaseLinkDevice
    {
        protected readonly int id;
        protected readonly LinkDeviceAFP afpDevice;

        internal LinkAFPRAMStick(LinkDeviceAFP device, byte channel, int id)
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

        private AFPThermistor thermistor;
        private AFPUsage usage;

        public override List<Sensor.BaseSensorDevice> GetSensors()
        {
            List<Sensor.BaseSensorDevice> ret = base.GetSensors();
            if (thermistor == null)
                thermistor = new AFPThermistor(this, 0);
            if (usage == null)
                usage = new AFPUsage(this, 0);
            ret.Add(thermistor);
            ret.Add(usage);
            return ret;
        }

        public class AFPThermistor : Usage
        {
            private readonly LinkAFPRAMStick afpDevice;
            internal AFPThermistor(LinkAFPRAMStick device, int id)
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

        public class AFPUsage : Usage
        {
            private readonly LinkAFPRAMStick afpDevice;
            internal AFPUsage(LinkAFPRAMStick device, int id)
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
