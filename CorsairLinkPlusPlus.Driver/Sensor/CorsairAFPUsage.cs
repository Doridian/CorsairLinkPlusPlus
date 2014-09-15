using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public class CorsairAFPUsage : CorsairUsage
    {
        internal CorsairAFPUsage(CorsairLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetSensorType()
        {
            return "Memory usage";
        }
    }
}
