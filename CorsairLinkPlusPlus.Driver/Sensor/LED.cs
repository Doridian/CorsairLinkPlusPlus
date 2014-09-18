using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class LED : BaseSensorDevice
    {
        byte[] rgbCache = null;

        internal LED(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public struct Color
        {
            public byte r, g, b;

            public Color(byte r, byte g, byte b)
            {
                this.r = r;
                this.g = g;
                this.b = b;
            }

            public override string ToString()
            {
                return r.ToString() + ", " + g.ToString() + ", " + b.ToString();
            }
        }

        public Color GetColor()
        {
            byte[] rgb = GetRGB();
            return new Color(rgb[0], rgb[1], rgb[2]);
        }

        internal override double GetValueInternal()
        {
            byte[] res = GetRGB();
            return BitConverter.ToUInt32(new byte[] { res[0], res[1], res[2], 0 }, 0);
        }

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

        public override string GetSensorType()
        {
            return "LED";
        }

        public override string GetUnit()
        {
            return "RGB";
        }
    }
}
