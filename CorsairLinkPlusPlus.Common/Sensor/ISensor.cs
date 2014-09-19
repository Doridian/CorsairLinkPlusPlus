using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Utility;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface ISensor : IDevice
    {
        double GetValue();
        SensorType GetSensorType();
        Unit GetUnit();
    }
}
