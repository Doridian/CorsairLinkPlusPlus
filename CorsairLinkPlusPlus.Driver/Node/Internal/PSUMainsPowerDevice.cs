using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.Sensor.Internal;
using CorsairLinkPlusPlus.Driver.USB;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Node.Internal
{
    class PSUMainsPowerDevice : PSUPrimaryPowerDevice
    {
        internal PSUMainsPowerDevice(LinkDevicePSU psuDevice, byte channel, string name)
            : base(psuDevice, channel, 0, name)
        {

        }

        protected override List<BaseDevice> GetSubDevicesInternal()
        {
            List<BaseDevice> ret = base.GetSubDevicesInternal();
            if (!(psuDevice is LinkDevicePSUHX))
                ret.Add(new PSUMainsPowerInSensor(this));
            return ret;
        }

        internal double ReadPowerIn()
        {
            if (psuDevice is LinkDevicePSUHX)
                return 0;
            return (ReadFloatInRegister(0x97) + (ReadCurrent() * ReadVoltage())) / 2.0;
        }

        internal override double ReadCurrent()
        {
            if (psuDevice is LinkDevicePSUHX)
                return ReadPower() / ReadVoltage();
            return ReadFloatInRegister(0x89);
        }

        internal override double ReadVoltage()
        {
            return ReadFloatInRegister(0x88);
        }

        internal override double ReadPower()
        {
            return ReadFloatInRegister(0xEE);
        }

        private double ReadFloatInRegister(byte register)
        {
            DisabledCheck();

            byte[] data;
            RootDevice.usbGlobalMutex.WaitOne();
            SetPage();
            data = psuDevice.ReadRegister(register, 2);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return BitCodec.ToFloat(BitConverter.ToUInt16(data, 0));
        }
    }
}
