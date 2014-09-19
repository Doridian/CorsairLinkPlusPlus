
namespace CorsairLinkPlusPlus.Driver.Utility
{
    public class CurvePoint<K, V>
    {
        public CurvePoint(K x, V y)
        {
            this.x = x;
            this.y = y;
        }

        public K x;
        public V y;

        public override string ToString()
        {
            return "{x=" + x + ",y=" + y + "}";
        }
    }
}
