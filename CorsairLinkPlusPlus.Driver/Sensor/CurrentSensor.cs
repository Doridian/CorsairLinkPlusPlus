using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class CurrentSensor : BasePowerSensor
    {
        internal CurrentSensor(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetSensorType()
        {
            return "Current";
        }

        public override string GetUnit()
        {
            return "A";
        }
    }
}
