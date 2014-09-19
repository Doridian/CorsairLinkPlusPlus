using CorsairLinkPlusPlus.Driver.CorsairLink.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    class PSUPrimaryPowerSensor : PowerSensor
    {
        private readonly PSUPrimaryPowerDevice powerDevice;

        internal PSUPrimaryPowerSensor(PSUPrimaryPowerDevice device)
            : base(device, 0)
        {
            this.powerDevice = device;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            powerDevice.Refresh(true);
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return powerDevice.ReadPower();
        }
    }
}
