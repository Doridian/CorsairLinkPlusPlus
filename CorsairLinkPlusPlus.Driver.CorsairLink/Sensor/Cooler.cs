using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class Cooler : BaseSensorDevice, ICooler
    {
        internal Cooler(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override Unit Unit
        {
            get
            {
                return Unit.RPM;
            }
        }
    }
}
