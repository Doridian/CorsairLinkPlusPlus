#region LICENSE
/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
 #endregion
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

        public static implicit operator ControlCurve<K, V>(Dictionary<string, object> value)
        {
            IEnumerable<object> pointsRaw = (IEnumerable<object>)value["Points"];
            List<CurvePoint<K, V>> points = new List<CurvePoint<K,V>>();
            foreach (object pointRaw in pointsRaw)
                points.Add((CurvePoint<K, V>)(dynamic)pointRaw);
            return new ControlCurve<K, V>(points);
        }
    }
}
