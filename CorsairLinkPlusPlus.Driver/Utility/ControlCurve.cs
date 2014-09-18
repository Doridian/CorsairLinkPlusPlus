using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Utility
{
    public class ControlCurveInt : ControlCurve<int>
    {
        public ControlCurveInt(List<CurvePoint<int>> points) : base(points) { }

        private int CalcYBetweenPoints(CurvePoint<int> curvePoint1, CurvePoint<int> curvePoint2, int x)
        {
            return (int)(curvePoint1.y + Map((float)x, (float)curvePoint1.x, (float)curvePoint2.x, 0.0f, 1.0f) * (curvePoint2.y - curvePoint1.y));
        }

        public int CalculateY(int x)
        {
            for (int i = 0; i < points.Count - 1; ++i)
                if (BetweenPoints(points[i], points[i + 1], x))
                    return CalcYBetweenPoints(points[i], points[i + 1], x);
            return -1;
        }
    }

    public class ControlCurve<T>
    {
        protected List<CurvePoint<T>> points;

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
