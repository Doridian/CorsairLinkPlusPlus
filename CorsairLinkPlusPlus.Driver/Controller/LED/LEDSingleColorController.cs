using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Controller.LED
{
    public class LEDSingleColorController : LEDColorCycleController
    {
        public LEDSingleColorController()
        {
            
        }

        public LEDSingleColorController(Color color) : base(new Color[] { color }) { }

        protected override int GetNumColors()
        {
            return 1;
        }

        public override byte GetLEDModernControllerID()
        {
            return 0x00;
        }
    }
}
