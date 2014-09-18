using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class Thermistor : BaseSensorDevice, IThermistor
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
