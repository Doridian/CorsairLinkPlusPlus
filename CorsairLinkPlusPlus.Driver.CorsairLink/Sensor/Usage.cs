using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
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
