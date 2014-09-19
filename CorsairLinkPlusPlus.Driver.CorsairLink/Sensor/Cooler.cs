using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class Cooler : BaseSensorDevice, ICooler
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
