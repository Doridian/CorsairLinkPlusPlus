using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
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

        public override SensorType SensorType
        {
            get
            {
                return SensorType.Power;
            }
        }
    }
}
