using CorsairLinkPlusPlus.Driver.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class PSUPrimaryCurrentSensor : CurrentSensor
    {
        private readonly PSUPrimaryPowerDevice powerDevice;

        internal PSUPrimaryCurrentSensor(PSUPrimaryPowerDevice device)
            : base(device, 0)
        {
            this.powerDevice = device;
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return powerDevice.ReadCurrent();
        }
    }
}
