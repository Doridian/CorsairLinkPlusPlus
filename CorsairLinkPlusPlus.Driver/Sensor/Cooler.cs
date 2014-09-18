using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
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
