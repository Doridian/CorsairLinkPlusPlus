using CorsairLinkPlusPlus.Driver.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class PSUPrimaryVoltageSensor : VoltageSensor
    {
        private readonly PSUPrimaryPowerDevice powerDevice;

        internal PSUPrimaryVoltageSensor(PSUPrimaryPowerDevice device)
            : base(device, 0)
        {
            this.powerDevice = device;
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return powerDevice.ReadVoltage();
        }
    }
}
