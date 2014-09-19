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

        public override string GetSensorType()
        {
            return "Voltage";
        }

        public override string GetUnit()
        {
            return "V";
        }
    }
}
