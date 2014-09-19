using CorsairLinkPlusPlus.Common.Utility;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface ILED : ISensor
    {
        Color GetColor();
        byte[] GetRGB();
    }
}
