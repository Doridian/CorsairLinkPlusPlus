using CorsairLinkPlusPlus.Driver.Node;
using System;
using CorsairLinkPlusPlus.Driver.Utility;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class Fan : Cooler
    {
        protected bool? cachedPWM = null;

        internal Fan(BaseLinkDevice device, int id)
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
        public abstract void SetMinimalRPM(int rpm);
        public abstract int GetMinimalRPM();

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
