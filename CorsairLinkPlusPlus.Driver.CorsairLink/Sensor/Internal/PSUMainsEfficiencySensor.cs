using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    public class PSUMainsEfficiencySensor : BasePowerSensor
    {
        private readonly PSUMainsPowerDevice powerDevice;

        internal PSUMainsEfficiencySensor(PSUMainsPowerDevice device)
            : base(device, 0)
        {
            powerDevice = device;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            powerDevice.Refresh(true);
        }

        public override SensorType GetSensorType()
        {
            return SensorType.Efficiency;
        }

        protected override double GetValueInternal()
        {
            return powerDevice.ReadEfficiency();
        }

        public override Unit GetUnit()
        {
            return Unit.Percent;
        }
    }
}
