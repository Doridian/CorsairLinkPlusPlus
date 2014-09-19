using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class Pump : Cooler, IPump
    {
        internal Pump(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override SensorType GetSensorType()
        {
            return SensorType.Pump;
        }
    }
}
