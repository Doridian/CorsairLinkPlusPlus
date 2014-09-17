using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanCustomCurveController : FanCurveController
    {
        public FanCustomCurveController() { }

        public FanCustomCurveController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public void SetCurve(ControlCurve curve)
        {
            this.curve = curve;
        }

        public override CurvePoint[] GetDefaultPoints()
        {
            return new CurvePoint[] {
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0)
            };
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
