using CorsairLinkPlusPlus.Driver.Utility;
using CorsairLinkPlusPlus.Driver.Controller;
using System;
using System.IO;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Common.Controller;

namespace CorsairLinkPlusPlus.Driver.Controller.LED
{
    public class LEDTemperatureController : TemperatureDependantControllerBase, LEDController, ICurveColorController
    {
        protected ControlCurve<double, Color> curve;

        public LEDTemperatureController()
        {

        }

        public LEDTemperatureController(ControlCurve<double, Color> colors)
        {
            SetCurve(colors);
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if(!(sensor is Sensor.LED))
                throw new ArgumentException();
            base.Apply(sensor);

            ((Sensor.LED)sensor).SetControlCurve(curve);
        }

        public void SetCurve(ControlCurve<double, Color> colors)
        {
            curve = colors;
        }

        public ControlCurve<double, Color> GetCurve()
        {
            return curve.Copy();
        }

        public virtual void AssignFrom(Sensor.LED led)
        {
            SetThermistor(((TemperatureControllableSensor)led).GetTemperatureSensor());
            curve = led.GetControlCurve();
        }

        protected int GetNumColors()
        {
            return 3;
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

        public byte GetLEDModernControllerID()
        {
            return 0xC0;
        }
    }
}
