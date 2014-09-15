using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public class CorsairFan : CorsairCooler
    {
        internal CorsairFan(CorsairLinkDevice device, int id)
            : base(device, id)
        {

        }

        public virtual bool IsPWM()
        {
            return false;
        }

        public override string GetSensorType()
        {
            return IsPWM() ? "PWM Fan" : "Fan";
        }
    }
}
