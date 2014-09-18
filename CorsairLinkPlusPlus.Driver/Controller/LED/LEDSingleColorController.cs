using CorsairLinkPlusPlus.Driver.Utility;

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
