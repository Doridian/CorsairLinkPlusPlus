using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Utility;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface ISensor : IDevice
    {
        double Value { get; }
        SensorType SensorType { get; }
        Unit Unit { get; }
    }
}
