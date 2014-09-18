using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Utility
{
    public class CurvePoint<T>
    {
        public CurvePoint(int x, T y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public T y;

        public override string ToString()
        {
            return "{x=" + x + ",y=" + y + "}";
        }
    }
}
