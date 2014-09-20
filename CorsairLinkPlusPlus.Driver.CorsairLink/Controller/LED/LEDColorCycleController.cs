using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller;
using System;
using System.IO;
using CorsairLinkPlusPlus.Common.Controller;
using System.Collections.Generic;
using CorsairLinkPlusPlus.Common.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED
{
    public abstract class LEDColorCycleController : ControllerBase, LEDController, IFixedColorCycleController
    {
        private readonly Color[] colors;

        public LEDColorCycleController()
        {
            int numColors = GetNumColors();
            this.colors = new Color[numColors];
            for (int i = 0; i < numColors; i++)
                this.colors[i] = new Color();
        }

        public LEDColorCycleController(Color[] colors) : this()
        {
            CopyNumColorArray(colors, this.colors);
        }

        public Color[] Value
        {
            get
            {
                Color[] ret = new Color[GetNumColors()];
                CopyNumColorArray(this.colors, ret);
                return ret;
            }
            set
            {
                CopyNumColorArray(value, this.colors);
            }
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if(!(sensor is Sensor.LED))
                throw new ArgumentException();
            base.Apply(sensor);

            MemoryStream stream = new MemoryStream();

            foreach(Color color in colors)
                stream.Write(color.ToArray(), 0, 3);

            ((Sensor.LED)sensor).SetFixedRGBCycleColors(stream.ToArray());
        }

        public Color[] GetCycle()
        {
            Color[] ret = new Color[GetNumColors()];
            CopyNumColorArray(this.colors, ret);
            return ret;
        }

        protected abstract int GetNumColors();

        public void AssignFrom(Sensor.LED led)
        {
            byte[] colorData = led.GetFixedRGBCycleColors();
            int numColors = GetNumColors();
            int j;
            for (int i = 0; i < numColors; i++)
            {
                j = i * 3;
                colors[i] = new Color(colorData[j], colorData[j + 1], colorData[j + 2]);
            }
        }

        private Color[] CopyNumColorArray(Color[] src, Color[] dst)
        {
            int numColors = GetNumColors();
            if (src.Length != numColors || dst.Length != numColors)
                throw new ArgumentException();

            for (int i = 0; i < numColors; i++)
                dst[i] = new Color(src[i]);

            return dst;
        }

        public abstract byte GetLEDModernControllerID();
    }
}
