using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanQuiteModeController : FanCurveController
    {
        public FanQuiteModeController() { }

        public FanQuiteModeController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public override CurvePoint<int>[] GetDefaultPoints()
        {
            return new CurvePoint<int>[] {
                new CurvePoint<int>(0, 0),
                new CurvePoint<int>(0, 0),
                new CurvePoint<int>(0, 0),
                new CurvePoint<int>(0, 0),
                new CurvePoint<int>(0, 0)
            };
        }

        public override byte GetFanModernControllerID()
        {
            return 0x08;
        }
    }
}
