
namespace CorsairLinkPlusPlus.Common.Utility
{
    public struct Color
    {
        public byte r, g, b;

        public Color(Color color)
        {
            this.r = color.r;
            this.g = color.g;
            this.b = color.b;
        }

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

        public byte[] ToArray()
        {
            return new byte[] { r, g, b };
        }
    }
}
