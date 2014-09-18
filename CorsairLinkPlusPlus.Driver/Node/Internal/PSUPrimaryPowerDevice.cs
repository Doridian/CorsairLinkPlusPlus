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
            this.psuDevice.Refresh(volatileOnly);
        }

        public override string GetLocalDeviceID()
        {
            return "PowerMain" + id;
        }

        public override string GetName()
        {
            return name;
        }

        protected override List<BaseDevice> GetSubDevicesInternal()
        {
            List<BaseDevice> sensors = base.GetSubDevicesInternal();
            sensors.Add(new PSUPrimaryCurrentSensor(this));
            sensors.Add(new PSUPrimaryPowerSensor(this));
            sensors.Add(new PSUPrimaryVoltageSensor(this));
            return sensors;
        }

        protected void SetPage()
        {
            psuDevice.SetMainPage(id);
        }

        internal virtual double ReadVoltage()
        {
            DisabledCheck();

            byte[] ret;

            RootDevice.usbGlobalMutex.WaitOne();
            SetPage();
            ret = ReadRegister(0x8B, 2);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return BitCodec.ToFloat(ret);
        }

        internal virtual double ReadCurrent()
        {
            DisabledCheck();

            byte[] ret;
            
            RootDevice.usbGlobalMutex.WaitOne();
            SetPage();
            ret = ReadRegister(0x8C, 2);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return BitCodec.ToFloat(ret);
        }

        internal virtual double ReadPower()
        {
            DisabledCheck();

            byte[] ret;

            RootDevice.usbGlobalMutex.WaitOne();
            SetPage();
            ret = ReadRegister(0x96, 2);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return BitCodec.ToFloat(ret);
        }
    }
}
