using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class VoltageSensor : BasePowerSensor
    {
        internal VoltageSensor(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override SensorType SensorType
        {
            get
            {
                return SensorType.Voltage;
            }
        }

        public override Unit Unit
        {
            get
            {
                return Unit.Volt;
            }
        }
    }
}
