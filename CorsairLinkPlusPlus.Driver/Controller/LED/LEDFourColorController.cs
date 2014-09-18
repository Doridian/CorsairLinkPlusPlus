using CorsairLinkPlusPlus.Driver.Utility;

namespace CorsairLinkPlusPlus.Driver.Controller.LED
{
    public class LEDFourColorController : LEDColorCycleController
    {
        public LEDFourColorController() { }

        public LEDFourColorController(Color[] colors) : base(colors) { }

        public LEDFourColorController(Color color1, Color color2, Color color3, Color color4) : base(new Color[] { color1, color2, color3, color4 }) { }

        protected override int GetNumColors()
        {
            return 4;
        }

        public override byte GetLEDModernControllerID()
        {
            return 0x80;
        }
    }
}
