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
        public override byte GetFanModernControllerID()
        {
            return 0x0A;
        }
    }
}
