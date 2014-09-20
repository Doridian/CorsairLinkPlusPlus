using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED
{
    public class LEDSingleColorController : LEDColorCycleController, IFixedColorController
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

        public Color Value
        {
            get
            {
                return GetCycle()[0];
            }
            set
            {
                SetCycle(new Color[] { value });
            }
        }
    }
}
