
namespace CorsairLinkPlusPlus.Common.Utility
{
    public class CurvePoint<K, V>
    {
        public CurvePoint()
        {

        }

        public CurvePoint(K x, V y)
        {
            this.X = x;
            this.Y = y;
        }

        public K X;
        public V Y;

        public override string ToString()
        {
            return "{x=" + X + ",y=" + Y + "}";
        }
    }
}
