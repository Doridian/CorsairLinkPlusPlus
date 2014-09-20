using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorsairLinkPlusPlus.Common.Utility
{
    public class ControlCurve<K, V>
    {
        protected List<CurvePoint<K, V>> points;

        public ControlCurve()
        {

        }

        public ControlCurve(params CurvePoint<K, V>[] points)
        {
            this.points = points.ToList<CurvePoint<K, V>>();
        }

        public ControlCurve(List<CurvePoint<K, V>> points)
        {
            this.points = points.ToList();
        }

        public List<CurvePoint<K, V>> Points
        {
            get
            {
                return points.ToList();
            }
            set
            {
                this.points = value.ToList();
            }
        }

        public ControlCurve<K, V> Copy()
        {
            return new ControlCurve<K, V>(points);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (CurvePoint<K, V> point in points)
                builder.Append(point).Append(", ");
            return builder.ToString();
        }
    }
}
