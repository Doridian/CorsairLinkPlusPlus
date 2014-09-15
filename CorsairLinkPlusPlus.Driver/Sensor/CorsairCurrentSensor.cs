using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class CorsairCurrentSensor : CorsairBasePowerSensor
    {
        internal CorsairCurrentSensor(CorsairLinkDevice device, int id)
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
