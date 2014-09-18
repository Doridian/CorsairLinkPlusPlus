using CorsairLinkPlusPlus.Driver.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class PSUMainsPowerInSensor : PowerSensor
    {
        private readonly PSUMainsPowerDevice powerDevice;

        internal PSUMainsPowerInSensor(PSUMainsPowerDevice device)
            : base(device, 0)
        {
            powerDevice = device;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            powerDevice.Refresh(true);
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return powerDevice.ReadPowerIn();
        }

        public override string GetSensorType()
        {
            return "Power In";
        }
    }
}
