using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface ISensor : IDevice
    {
        double Value { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        SensorType SensorType { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        Unit Unit { get; }
    }
}
