
using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.Sensor.Internal;
using CorsairLinkPlusPlus.Driver.USB;
using System.Collections.Generic;
namespace CorsairLinkPlusPlus.Driver.Node.Internal
{
    class PSUMainPowerDevice : BaseLinkDevice
    {
        protected readonly int id;
        protected readonly string name;
        protected readonly LinkDevicePSU psuDevice;
        internal PSUMainPowerDevice(LinkDevicePSU psuDevice, byte channel, int id, string name)
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
            sensors.Add(new PSUMainCurrentSensor(this));
            sensors.Add(new PSUMainPowerSensor(this));
            sensors.Add(new PSUMainVoltageSensor(this));
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

            RootDevice.usbGlobalMutex.WaitOne();
            SetPage();
            ret = ReadRegister(0x8B, 2);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return BitCodec.ToFloat(ret);
        }

        internal double ReadCurrent()
        {
            DisabledCheck();

            byte[] ret;
            
            RootDevice.usbGlobalMutex.WaitOne();
            SetPage();
            ret = ReadRegister(0x8C, 2);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return BitCodec.ToFloat(ret);
        }

        internal double ReadPower()
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
