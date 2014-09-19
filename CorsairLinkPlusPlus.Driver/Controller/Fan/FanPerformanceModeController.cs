using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.Utility;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanPerformanceModeController : FanCurveController
    {
        public FanPerformanceModeController() { }

        public FanPerformanceModeController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public override ControlCurve<double, double> GetDefaultPoints()
        {
            return new ControlCurve<double, double>(
                new CurvePoint<double, double>(25, 1600),
                new CurvePoint<double, double>(28, 1700),
                new CurvePoint<double, double>(31, 2200),
                new CurvePoint<double, double>(34, 2400),
                new CurvePoint<double, double>(40, 2700)
            );
        }
        public override byte GetFanModernControllerID()
        {
            return 0x0C;
        }
    }
}
