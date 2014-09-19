using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class BaseSensorDevice : CorsairLinkPlusPlus.Common.Sensor.BaseSensorDevice, ISensor
    {
        internal readonly BaseLinkDevice device;
        internal readonly int id;

        internal BaseSensorDevice(BaseLinkDevice device, int id)
            : base(device)
        {
            this.device = device;
            this.id = id;
        }

        public override string GetLocalDeviceID()
        {
            return "Sensor" + GetSensorType() + id;
        }

        public override string GetName()
        {
            return GetSensorType() + " " + id;
        }
    }
}
