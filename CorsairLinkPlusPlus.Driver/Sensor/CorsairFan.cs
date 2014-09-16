using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class CorsairFan : CorsairCooler
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

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            if (!volatileOnly)
                cachedPWM = null;
        }

        public abstract void SetRPM(int fixedRPM);

        public abstract void SetPercent(int percent);

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
