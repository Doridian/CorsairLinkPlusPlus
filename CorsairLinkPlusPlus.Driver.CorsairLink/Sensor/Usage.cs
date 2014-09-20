using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class Usage : BaseSensorDevice
    {
        internal Usage(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override SensorType SensorType
        {
            get
            {
                return SensorType.Usage;
            }
        }

        public override Unit Unit
        {
            get
            {
                return Unit.Percent;
            }
        }
    }
}
