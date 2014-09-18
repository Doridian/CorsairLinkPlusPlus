using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.Sensor;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node.Internal
{
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

        public override string GetLocalDeviceID()
        {
            return "DIMM" + id;
        }

        public override string GetName()
        {
            return "Corsair AirFlow Pro RAM stick " + id;
        }

        internal byte[] GetReadings()
        {
            DisabledCheck();
            return ReadRegister((byte)(id << 4), 16);
        }

        public override bool IsPresent()
        {
            return GetReadings()[0] != 0;
        }

        protected override List<BaseDevice> GetSubDevicesInternal()
        {
            List<BaseDevice> ret = base.GetSubDevicesInternal();
            ret.Add(new AFPThermistor(this, 0));
            ret.Add(new AFPUsage(this, 0));
            return ret;
        }

        public class AFPThermistor : Thermistor
        {
            private readonly LinkAFPRAMStick afpDevice;
            internal AFPThermistor(LinkAFPRAMStick device, int id)
                : base(device, id)
            {
                this.afpDevice = device;
            }

            internal override double GetValueInternal()
            {
                DisabledCheck();
                return 1;
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
                DisabledCheck();
                return afpDevice.GetReadings()[2];
            }

            public override string GetSensorType()
            {
                return "Memory usage";
            }
        }
    }
}
