using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Utility
{
    class PowerCurves
    {
        internal static double Interpolate(ref double powerOut, ref double powerIn, double voltage, string psuName)
        {
            bool flag = psuName.StartsWith("HX");
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            if (flag)
            {
                num = powerOut;
                num2 = num * num;
            }
            else
            {
                num3 = powerIn;
                num4 = num3 * num3;
            }
            switch (psuName)
            {
                case "AX1200i":
                    num = ((voltage < 180.0) ? (-5.398236E-05 * num4 + 0.9791846 * num3 - 14.75333) : (-3.724072E-05 * num4 + 0.9792162 * num3 - 13.2167));
                    goto IL_3B1;
                case "AX860i":
                    num = ((voltage < 180.0) ? (-7.803814E-05 * num4 + 0.9856293 * num3 - 12.32274) : (-6.501415E-05 * num4 + 0.9914832 * num3 - 11.86228));
                    goto IL_3B1;
                case "AX760i":
                    num = ((voltage < 180.0) ? (-8.497113E-05 * num4 + 0.9882959 * num3 - 12.28865) : (-6.94856E-05 * num4 + 0.9947551 * num3 - 12.14068));
                    goto IL_3B1;
                case "HX1000i":
                    num3 = ((voltage < 180.0) ? (8.614363E-05 * num2 + 1.013681 * num + 12.57297) : (6.625963E-05 * num2 + 1.013373 * num + 11.67543));
                    goto IL_3B1;
                case "HX850i":
                    num3 = ((voltage < 180.0) ? (9.402802E-05 * num2 + 1.01568 * num + 12.40252) : (6.333025E-05 * num2 + 1.023295 * num + 9.813914));
                    goto IL_3B1;
                case "HX750i":
                    num3 = ((voltage < 180.0) ? (0.0001011842 * num2 + 1.015511 * num + 12.08922) : (7.757325E-05 * num2 + 1.015346 * num + 10.91073));
                    goto IL_3B1;
                case "HX650i":
                    num3 = ((voltage < 180.0) ? (0.0001215054 * num2 + 1.015551 * num + 9.62592) : (8.296032E-05 * num2 + 1.018958 * num + 8.411756));
                    goto IL_3B1;
            }
            num = ((voltage < 180.0) ? (-4.537257E-05 * num4 + 0.9955244 * num3 - 17.04104) : (-4.024153E-05 * num4 + 1.002732 * num3 - 19.12843));
        IL_3B1:
            double num6 = num / num3;
            bool flag2 = false;
            if (num6 > 0.99)
            {
                num6 = 0.99;
                flag2 = true;
            }
            else
            {
                if (num6 < 0.4)
                {
                    num6 = 0.4;
                    flag2 = true;
                }
            }
            if (flag2)
            {
                if (flag)
                {
                    num3 = num / num6;
                }
                else
                {
                    num = num3 * num6;
                }
            }
            powerOut = num;
            powerIn = num3;
            return num6 * 100.0;
        }
    }
}
