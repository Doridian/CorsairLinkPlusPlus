using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class CurrentSensor : BasePowerSensor
    {
        internal CurrentSensor(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override SensorType SensorType
        {
            get
            {
                return SensorType.Current;
            }
        }

        public override Unit Unit
        {
            get
            {
                return Unit.Ampere;
            }
        }
    }
}
