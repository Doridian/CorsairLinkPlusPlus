using CorsairLinkPlusPlus.Driver.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class PSUPrimaryPowerSensor : PowerSensor
    {
        private readonly PSUPrimaryPowerDevice powerDevice;

        internal PSUPrimaryPowerSensor(PSUPrimaryPowerDevice device)
            : base(device, 0)
        {
            this.powerDevice = device;
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return powerDevice.ReadPower();
        }
    }
}
