using CorsairLinkPlusPlus.Common.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED
{
    public class LEDTwoColorController : LEDColorCycleController
    {
        public LEDTwoColorController() { }

        public LEDTwoColorController(Color[] colors) : base(colors) { }

        public LEDTwoColorController(Color color1, Color color2) : base(new Color[] { color1, color2 }) { }

        protected override int GetNumColors()
        {
            return 2;
        }

        public override byte GetLEDModernControllerID()
        {
            return 0x40;
        }
    }
}
