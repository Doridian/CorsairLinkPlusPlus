using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class CorsairLED : CorsairSensor
    {
        internal CorsairLED(CorsairLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetSensorType()
        {
            return "LED";
        }

        public override string GetUnit()
        {
            return "RGB";
        }
    }
}
