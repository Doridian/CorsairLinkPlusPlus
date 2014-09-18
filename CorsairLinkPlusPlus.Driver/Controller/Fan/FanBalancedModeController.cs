using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanBalancedModeController : FanCurveController
    {
        public FanBalancedModeController() { }

        public FanBalancedModeController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public override ControlCurve<int> GetDefaultPoints()
        {
            return new ControlCurve<int>(
                new CurvePoint<int>(25, 1300),
                new CurvePoint<int>(28, 1500),
                new CurvePoint<int>(31, 1800),
                new CurvePoint<int>(37, 1900),
                new CurvePoint<int>(40, 2000)
            );
        }
        public override byte GetFanModernControllerID()
        {
            return 0x0A;
        }
    }
}
