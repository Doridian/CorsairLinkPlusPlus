using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.Utility;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanCustomCurveController : FanCurveController
    {
        public FanCustomCurveController() { }

        public FanCustomCurveController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public void SetCurve(ControlCurve<int> curve)
        {
            this.curve = curve;
        }

        public override ControlCurve<int> GetDefaultPoints()
        {
            return new ControlCurve<int>(
                new CurvePoint<int>(0, 0),
                new CurvePoint<int>(0, 0),
                new CurvePoint<int>(0, 0),
                new CurvePoint<int>(0, 0),
                new CurvePoint<int>(0, 0)
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
            ((Sensor.Fan)sensor).SetControlCurve(GetCurve());
        }

        public override byte GetFanModernControllerID()
        {
            return 0x0E;
        }
    }

}
