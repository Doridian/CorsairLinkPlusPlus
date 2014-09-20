
namespace CorsairLinkPlusPlus.Common.Utility
{
    public struct Color
    {
        public byte R, G, B;

        public Color(Color color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public Color(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public override string ToString()
        {
            return R.ToString() + ", " + G.ToString() + ", " + B.ToString();
        }

        public byte[] ToArray()
        {
            return new byte[] { R, G, B };
        }
    }
}
