using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class BasePowerSensor : BaseSensorDevice
    {
        internal BasePowerSensor(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }
    }
}
