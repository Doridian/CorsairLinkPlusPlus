using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.Sensor.Internal;
using CorsairLinkPlusPlus.Driver.USB;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node.Internal
{
    class PSUPrimaryPowerDevice : BaseLinkDevice
    {
        protected readonly int id;
        protected readonly string name;
        protected readonly LinkDevicePSU psuDevice;
        protected double cachedVoltage = double.NaN;
        protected double cachedCurrent = double.NaN;
        protected double cachedPower = double.NaN;

        internal PSUPrimaryPowerDevice(LinkDevicePSU psuDevice, byte channel, int id, string name)
            : base(psuDevice.usbDevice, channel)
        {
            this.id = id;
            this.name = name;
            this.psuDevice = psuDevice;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            this.psuDevice.Refresh(true);
            cachedCurrent = double.NaN;
            cachedVoltage = double.NaN;
            cachedPower = double.NaN;
        }

        public override string GetLocalDeviceID()
        {
            return "PowerMain" + id;
        }

        public override string GetName()
        {
            return name;
        }

        protected override List<IDevice> GetSubDevicesInternal()
        {
            List<IDevice> sensors = base.GetSubDevicesInternal();
            sensors.Add(new PSUPrimaryCurrentSensor(this));
            sensors.Add(new PSUPrimaryPowerSensor(this));
            sensors.Add(new PSUPrimaryVoltageSensor(this));
            return sensors;
        }

        protected void SetPage()
        {
            psuDevice.SetMainPage(id);
        }

        protected virtual void ReadValues()
        {
            byte[] retVoltage, retCurrent, retPower;

            RootDevice.usbGlobalMutex.WaitOne();
            SetPage();
            retVoltage = ReadRegister(0x8B, 2);
            retCurrent = ReadRegister(0x8C, 2);
            retPower = ReadRegister(0x96, 2);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            cachedVoltage = BitCodec.ToFloat(retVoltage);
            cachedCurrent = BitCodec.ToFloat(retCurrent);
            cachedPower = BitCodec.ToFloat(retPower);
        }

        internal virtual double ReadVoltage()
        {
            DisabledCheck();

            if (double.IsNaN(cachedVoltage))
                ReadValues();

            return cachedVoltage;
        }

        internal virtual double ReadCurrent()
        {
            DisabledCheck();

            if (double.IsNaN(cachedCurrent))
                ReadValues();

            return cachedCurrent;
        }

        internal virtual double ReadPower()
        {
            DisabledCheck();

            if (double.IsNaN(cachedPower))
                ReadValues();

            return cachedPower;
        }
    }
}
