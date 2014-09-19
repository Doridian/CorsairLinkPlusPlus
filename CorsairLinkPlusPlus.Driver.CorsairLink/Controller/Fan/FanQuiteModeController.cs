using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public class FanQuiteModeController : FanCurveController
    {
        public FanQuiteModeController() { }

        public FanQuiteModeController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public override ControlCurve<double, double> GetDefaultPoints()
        {
            return new ControlCurve<double, double>(
                new CurvePoint<double, double>(28, 900),
                new CurvePoint<double, double>(31, 1000),
                new CurvePoint<double, double>(34, 1100),
                new CurvePoint<double, double>(37, 1200),
                new CurvePoint<double, double>(40, 1300)
            );
        }

        public override byte GetFanModernControllerID()
        {
            return 0x08;
        }
    }
}
