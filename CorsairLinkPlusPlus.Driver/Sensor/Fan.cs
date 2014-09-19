using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Utility;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class Fan : Cooler, IFan
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

        internal abstract void SetFixedRPM(double fixedRPM);
        internal abstract void SetFixedPercent(double percent);
        internal abstract double GetFixedRPM();
        internal abstract double GetFixedPercent();
        internal abstract ControlCurve<double, double> GetControlCurve();
        internal abstract void SetControlCurve(ControlCurve<double, double> curve);
        internal abstract void SetMinimalRPM(double rpm);
        internal abstract double GetMinimalRPM();

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
