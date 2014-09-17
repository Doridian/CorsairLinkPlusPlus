using CorsairLinkPlusPlus.Driver.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class PSUMainPowerSensor : PowerSensor
    {
        private readonly PSUMainPowerDevice powerDevice;

        internal PSUMainPowerSensor(PSUMainPowerDevice device)
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
}
