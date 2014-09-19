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
        protected double cachedPowerIn = double.NaN;
        protected double cachedEfficiency = double.NaN;

        internal PSUMainsPowerDevice(LinkDevicePSU psuDevice, byte channel, string name)
            : base(psuDevice, channel, 0, name)
        {

        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            cachedPowerIn = double.NaN;
            cachedEfficiency = double.NaN;
        }

        protected override List<IDevice> GetSubDevicesInternal()
        {
            List<IDevice> ret = base.GetSubDevicesInternal();
            if (!(psuDevice is LinkDevicePSUHX))
                ret.Add(new PSUMainsPowerInSensor(this));
            ret.Add(new PSUMainsEfficiencySensor(this));
            return ret;
        }

        internal double ReadPowerIn()
        {
            if (double.IsNaN(cachedPowerIn))
                ReadValues();
            return cachedPowerIn;
        }

        internal double ReadEfficiency()
        {
            if (double.IsNaN(cachedEfficiency))
                ReadValues();
            return cachedEfficiency;
        }

        protected override void ReadValues()
        {
            byte[] retVoltage, retCurrent, retPower, retPowerIn;

            RootDevice.usbGlobalMutex.WaitOne();
            SetPage();
            retVoltage = ReadRegister(0x88, 2);
            if (psuDevice is LinkDevicePSUHX)
            {
                retCurrent = null;
                retPowerIn = null;
            }
            else
            {
                retCurrent = ReadRegister(0x89, 2);
                retPowerIn = ReadRegister(0x97, 2);
            }
            retPower = ReadRegister(0xEE, 2);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            cachedVoltage = BitCodec.ToFloat(retVoltage);
            cachedPower = BitCodec.ToFloat(retPower);
            if (retCurrent == null)
                cachedCurrent = cachedPower / cachedVoltage;
            else
                cachedCurrent = BitCodec.ToFloat(retCurrent);
            if (retPowerIn == null)
                cachedPowerIn = 0;
            else
                cachedPowerIn = (BitCodec.ToFloat(retPowerIn) + (cachedCurrent * cachedVoltage)) / 2.0;

            PowerData adjustedData = PowerCurves.Interpolate(new PowerData(cachedPowerIn, cachedPower), cachedVoltage, psuDevice);

            cachedEfficiency = adjustedData.efficiency;
            cachedPowerIn = adjustedData.powerIn;
            cachedPower = adjustedData.powerOut;
        }
    }
}
