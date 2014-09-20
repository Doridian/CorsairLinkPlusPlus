using CorsairLinkPlusPlus.Common.Utility;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface ILED : ISensor
    {
        Color Color { get; }
        byte[] GetRGB();
    }
}
