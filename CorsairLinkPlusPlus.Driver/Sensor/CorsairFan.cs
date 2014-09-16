using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Utility;
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

        public abstract void SetFixedRPM(int fixedRPM);
        public abstract void SetFixedPercent(int percent);
        public abstract int GetFixedRPM();
        public abstract int GetFixedPercent();
        public abstract ControlCurve GetControlCurve();
        public abstract void SetControlCurve(ControlCurve curve);

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
