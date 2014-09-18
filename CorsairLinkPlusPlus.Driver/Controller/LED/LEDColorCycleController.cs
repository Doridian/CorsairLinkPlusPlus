using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Controller.LED
{
    public class LEDColorCycleController
    {
        private readonly Color[] colors;

        public LEDColorCycleController(int numColors)
        {
            this.colors = new Color[numColors];
        }

        public LEDColorCycleController(Color[] colors)
        {
            this.colors = new Color[colors.Length];
            colors.CopyTo(this.colors, 0);
        }
    }
}
