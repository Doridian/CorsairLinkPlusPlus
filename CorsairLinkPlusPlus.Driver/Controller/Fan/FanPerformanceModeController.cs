using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanPerformanceModeController : FanCurveController
    {
        public FanPerformanceModeController() { }

        public FanPerformanceModeController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public override ControlCurve<int> GetDefaultPoints()
        {
            return new ControlCurve<int>(
                new CurvePoint<int>(25, 1600),
                new CurvePoint<int>(28, 1700),
                new CurvePoint<int>(31, 2200),
                new CurvePoint<int>(34, 2400),
                new CurvePoint<int>(40, 2700)
            );
        }
        public override byte GetFanModernControllerID()
        {
            return 0x0C;
        }
    }
}
