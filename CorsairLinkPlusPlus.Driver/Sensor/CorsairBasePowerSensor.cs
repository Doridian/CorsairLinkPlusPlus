using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class CorsairBasePowerSensor : CorsairSensor
    {
        internal CorsairBasePowerSensor(CorsairLinkDevice device, int id)
            : base(device, id)
        {

        }
    }
}
