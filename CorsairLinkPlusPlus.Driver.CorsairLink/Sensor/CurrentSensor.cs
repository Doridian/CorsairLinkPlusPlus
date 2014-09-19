using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
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
