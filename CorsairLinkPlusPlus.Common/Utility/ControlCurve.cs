using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Utility
{
    public class ControlCurve<T>
    {
        protected List<CurvePoint<T>> points;

        public ControlCurve(params CurvePoint<T>[] points)
        {
            this.points = points.ToList<CurvePoint<T>>();
        }

        public ControlCurve(List<CurvePoint<T>> points)
        {
            this.points = points.ToList();
        }

        public List<CurvePoint<T>> GetPoints()
        {
            return points.ToList();
        }

        protected bool BetweenPoints(CurvePoint<T> a, CurvePoint<T> b, int x)
        {
            return x >= a.x && x <= b.x;
        }

        protected float Map(float val, float min, float max, float newMin, float newMax)
        {
            return (val - min) / (max - min) * (newMax - newMin) + newMin;
        }

        public ControlCurve<T> Copy()
        {
            return new ControlCurve<T>(points);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (CurvePoint<T> point in points)
                builder.Append(point).Append(", ");
            return builder.ToString();
        }
    }
}
