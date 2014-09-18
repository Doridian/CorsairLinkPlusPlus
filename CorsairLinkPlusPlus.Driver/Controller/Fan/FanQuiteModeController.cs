using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.Utility;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanQuiteModeController : FanCurveController
    {
        public FanQuiteModeController() { }

        public FanQuiteModeController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public override ControlCurve<int> GetDefaultPoints()
        {
            return new ControlCurve<int>(
                new CurvePoint<int>(28, 900),
                new CurvePoint<int>(31, 1000),
                new CurvePoint<int>(34, 1100),
                new CurvePoint<int>(37, 1200),
                new CurvePoint<int>(40, 1300)
            );
        }

        public override byte GetFanModernControllerID()
        {
            return 0x08;
        }
    }
}
