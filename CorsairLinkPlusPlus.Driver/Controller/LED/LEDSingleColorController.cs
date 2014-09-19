using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.Utility;

namespace CorsairLinkPlusPlus.Driver.Controller.LED
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

        public void SetValue(Color value)
        {
            SetCycle(new Color[] { value });
        }

        public Color GetValue()
        {
            return GetCycle()[0];
        }
    }
}
