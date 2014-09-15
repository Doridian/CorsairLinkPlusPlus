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
                CorsairLinkAFPRAMStick ramStick = new CorsairLinkAFPRAMStick(usbDevice, channel, i);
                if(ramStick.IsPresent())
                    ramSticks.Add(ramStick);
            }
            return ramSticks;
        }
    }
    public class CorsairLinkAFPRAMStick : CorsairLinkDevice
    {
        protected readonly int id;

        internal CorsairLinkAFPRAMStick(CorsairLinkUSBDevice usbDevice, byte channel, int id) : base(usbDevice, channel)
        {
            this.id = id;
        }

        public override string GetUDID()
        {
            return base.GetUDID() + "/DIMM" + id;
        }

        public override string GetName()
        {
            return "Corsair AirFlow Pro RAM stick " + id;
        }

        private byte[] GetReadings()
        {
            return ReadRegister((byte)(id << 4), 16);
        }

        public new bool IsPresent()
        {
            return GetReadings()[0] != 0;
        }

        internal override double GetTemperatureDegC(int _id)
        {
            if (_id != 0)
                return 0;
            return GetReadings()[2];
        }

        internal override double GetUsagePercent(int _id)
        {
            if (_id != 0)
                return 0;
            return 1;
        }

        public override int GetTemperatureCount()
        {
            return 1;
        }

        public override List<CorsairSensor> GetSensors()
        {
            List<CorsairSensor> ret = base.GetSensors();
            ret.Add(new CorsairAFPUsage(this, 0));
            return ret;
        }
    }
}
