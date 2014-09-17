using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class Thermistor : BaseSensorDevice
    {
        internal Thermistor(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetSensorType()
        {
            return "Temp";
        }

        public override string GetUnit()
        {
            return "°C";
        }
    }
}
