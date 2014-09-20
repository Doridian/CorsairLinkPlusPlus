using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class LED : BaseSensorDevice, ILED
    {
        byte[] rgbCache = null;

        internal LED(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public Color Color
        {
            get
            {
                byte[] rgb = GetRGB();
                return new Color(rgb[0], rgb[1], rgb[2]);
            }
        }

        protected override double GetValueInternal()
        {
            byte[] res = GetRGB();
            return BitConverter.ToUInt32(new byte[] { res[0], res[1], res[2], 0 }, 0);
        }

        internal abstract byte[] GetFixedRGBCycleColors();

        internal abstract void SetFixedRGBCycleColors(byte[] colors);

        internal abstract ControlCurve<double, Color> GetControlCurve();

        internal abstract void SetControlCurve(ControlCurve<double, Color> colors);

        public byte[] GetRGB()
        {
            if (rgbCache == null)
                rgbCache = GetRGBInternal();
            return rgbCache;
        }

        internal void SetColor(Color color)
        {
            SetRGB(new byte[] { color.r, color.g, color.b });
        }

        internal abstract void SetRGB(byte[] rgb);

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            rgbCache = null;
        }

        internal abstract byte[] GetRGBInternal();

        public override SensorType SensorType
        {
            get
            {
                return SensorType.LED;
            }
        }

        public override Unit Unit
        {
            get
            {
                return Unit.Color;
            }
        }
    }
}
