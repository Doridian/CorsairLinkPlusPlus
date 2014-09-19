using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public class FanBalancedModeController : FanCurveController
    {
        public FanBalancedModeController() { }

        public FanBalancedModeController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public override ControlCurve<double, double> GetDefaultPoints()
        {
            return new ControlCurve<double, double>(
                new CurvePoint<double, double>(25, 1300),
                new CurvePoint<double, double>(28, 1500),
                new CurvePoint<double, double>(31, 1800),
                new CurvePoint<double, double>(37, 1900),
                new CurvePoint<double, double>(40, 2000)
            );
        }
        public override byte GetFanModernControllerID()
        {
            return 0x0A;
        }
    }
}
