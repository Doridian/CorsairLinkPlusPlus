using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class PowerSensor : BasePowerSensor
    {
        internal PowerSensor(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override SensorType GetSensorType()
        {
            return SensorType.Power;
        }

        public override Unit GetUnit()
        {
            return Unit.Watt;
        }
    }
}
