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
