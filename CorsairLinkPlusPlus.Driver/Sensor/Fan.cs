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

        internal abstract void SetFixedRPM(int fixedRPM);
        internal abstract void SetFixedPercent(int percent);
        internal abstract int GetFixedRPM();
        internal abstract int GetFixedPercent();
        internal abstract ControlCurve<int> GetControlCurve();
        internal abstract void SetControlCurve(ControlCurve<int> curve);
        internal abstract void SetMinimalRPM(int rpm);
        internal abstract int GetMinimalRPM();

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
