using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public class CorsairFan : CorsairCooler
    {
        protected bool? cachedPWM = null;

        internal CorsairFan(CorsairLinkDevice device, int id)
            : base(device, id)
        {

        }

        public bool IsPWM()
        {
            if (cachedPWM == null)
                cachedPWM = IsPWMInternal();
            return (bool)cachedPWM;
        }

        public override void Refresh()
        {
            base.Refresh();
            cachedPWM = null;
        }

        internal virtual bool IsPWMInternal()
        {
            return false;
        }

        public override string GetSensorType()
        {
            return IsPWM() ? "PWM Fan" : "Fan";
        }
    }
}
