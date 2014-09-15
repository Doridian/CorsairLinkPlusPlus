using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class CorsairVoltageSensor : CorsairBasePowerSensor
    {
        internal CorsairVoltageSensor(CorsairLinkDevice device, int id)
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
