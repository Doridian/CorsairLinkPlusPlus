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
}
