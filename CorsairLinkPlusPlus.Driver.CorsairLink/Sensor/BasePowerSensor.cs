using CorsairLinkPlusPlus.Driver.CorsairLink.Node;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class BasePowerSensor : BaseSensorDevice
    {
        internal BasePowerSensor(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }
    }
}
