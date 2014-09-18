using CorsairLinkPlusPlus.Driver.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class PSUMainVoltageSensor : VoltageSensor
    {
        private readonly PSUMainPowerDevice powerDevice;

        internal PSUMainVoltageSensor(PSUMainPowerDevice device)
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
