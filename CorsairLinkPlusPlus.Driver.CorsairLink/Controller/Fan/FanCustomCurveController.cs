using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public class FanCustomCurveController : FanCurveController
    {
        public FanCustomCurveController() { }

        public FanCustomCurveController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public override void SetCurve(ControlCurve<double, double> curve)
        {
            this.curve = curve;
        }

        public override ControlCurve<double, double> GetDefaultPoints()
        {
            return new ControlCurve<double, double>(
                new CurvePoint<double, double>(0, 0),
                new CurvePoint<double, double>(0, 0),
                new CurvePoint<double, double>(0, 0),
                new CurvePoint<double, double>(0, 0),
                new CurvePoint<double, double>(0, 0)
            );
        }

        public override void AssignFrom(Sensor.Fan fan)
        {
            base.AssignFrom(fan);
            SetCurve(fan.GetControlCurve());
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            base.Apply(sensor);
            ((Sensor.Fan)sensor).SetControlCurve(Value);
        }

        public override byte GetFanModernControllerID()
        {
            return 0x0E;
        }
    }

}
