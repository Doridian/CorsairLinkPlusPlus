
namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface IFan : ICooler
    {
        bool PWM { get; }
    }
}
