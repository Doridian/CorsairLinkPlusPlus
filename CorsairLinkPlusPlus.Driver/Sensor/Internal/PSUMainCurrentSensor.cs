using CorsairLinkPlusPlus.Driver.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class PSUMainCurrentSensor : CurrentSensor
    {
        private readonly PSUMainPowerDevice powerDevice;

        internal PSUMainCurrentSensor(PSUMainPowerDevice device)
            : base(device, 0)
        {
            this.powerDevice = device;
        }

        internal override double GetValueInternal()
        {
            DisabledCheck();

            return powerDevice.ReadCurrent();
        }
    }
}
