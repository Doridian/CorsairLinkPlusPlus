using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class CorsairUsage : CorsairSensor
    {
        internal CorsairUsage(CorsairLinkDevice device, int id)
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
