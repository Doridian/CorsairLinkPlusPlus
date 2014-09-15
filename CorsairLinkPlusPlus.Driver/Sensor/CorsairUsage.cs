using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public class CorsairUsage : CorsairSensor
    {
        internal CorsairUsage(CorsairLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetSensorType()
        {
            return "Usage";
        }

        public override double GetValue()
        {
            return device.GetUsagePercent(id);
        }

        public override string GetUnit()
        {
            return "%";
        }
    }
}
