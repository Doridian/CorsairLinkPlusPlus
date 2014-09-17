using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class Usage : BaseSensorDevice
    {
        internal Usage(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetSensorType()
        {
            return "Usage";
        }

        public override string GetUnit()
        {
            return "%";
        }
    }
}
