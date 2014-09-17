using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class PowerSensor : BasePowerSensor
    {
        internal PowerSensor(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetSensorType()
        {
            return "Power";
        }

        public override string GetUnit()
        {
            return "W";
        }
    }
}
