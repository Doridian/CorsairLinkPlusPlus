using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class Cooler : BaseSensorDevice
    {
        internal Cooler(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetUnit()
        {
            return "RPM";
        }
    }
}
