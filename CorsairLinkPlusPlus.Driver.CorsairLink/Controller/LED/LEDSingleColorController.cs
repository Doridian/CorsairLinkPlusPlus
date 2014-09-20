using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED
{
    public class LEDSingleColorController : ControllerBase, LEDController, IFixedColorController
    {
        private Color m_color;

        public LEDSingleColorController()
        {
            
        }

        public LEDSingleColorController(Color color)
        {
            m_color = color;
        }

        public byte GetLEDModernControllerID()
        {
            return 0x00;
        }

        public Color Value
        {
            get
            {
                return m_color;
            }
            set
            {
                m_color = value;
            }
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if (!(sensor is Sensor.LED))
                throw new ArgumentException();
            base.Apply(sensor);

            ((Sensor.LED)sensor).SetFixedRGBCycleColors(m_color.ToArray());
        }

        public void AssignFrom(Sensor.LED led)
        {
            byte[] colorData = led.GetFixedRGBCycleColors();
            m_color = new Color(colorData[0], colorData[1], colorData[2]);
        }
    }
}
