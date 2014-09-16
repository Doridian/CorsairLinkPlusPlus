using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Utility
{
    public class CurvePoint
    {
        public CurvePoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x, y;

        public override string ToString()
        {
            return "{x=" + x + ",y=" + y + "}";
        }
    }

    public class ControlCurve
    {
        List<CurvePoint> points;

        public ControlCurve(List<CurvePoint> points)
        {
            this.points = points.ToList();
        }

        public List<CurvePoint> GetPoints()
        {
            return points.ToList();
        }

        private bool BetweenPoints(CurvePoint a, CurvePoint b, int x)
        {
            return x >= a.x && x <= b.x;
        }

        private float Map(float val, float min, float max, float newMin, float newMax)
        {
            return (val - min) / (max - min) * (newMax - newMin) + newMin;
        }

        private int CalcYBetweenPoints(CurvePoint curvePoint1, CurvePoint curvePoint2, int x)
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

        public ControlCurve Copy()
        {
            return new ControlCurve(points);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach(CurvePoint point in points)
                builder.Append(point).Append(", ");
            return builder.ToString();
        }
    }
}
