using CorsairLinkPlusPlus.Common;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface ISensor : IDevice
    {
        double GetValue();
        string GetSensorType();
        string GetUnit();
    }
}
