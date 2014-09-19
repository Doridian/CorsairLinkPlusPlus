using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Node.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Utility
{

    internal struct PowerData
    {
        internal PowerData(double powerIn, double powerOut, double efficiency = 0.0)
        {
            this.powerIn = powerIn;
            this.powerOut = powerOut;
            this.efficiency = efficiency;
        }
        internal double powerOut, powerIn, efficiency;
    }

    class PowerCurves
    {
        internal static PowerData Interpolate(PowerData rawData, double voltage, LinkDevicePSU psu)
        {
            bool psuIsHX = psu is LinkDevicePSUHX;
            double powerOut = 0.0;
            double powerOutSquare = 0.0;
            double powerIn = 0.0;
            double powerInSquare = 0.0;
            if (psuIsHX)
            {
                powerOut = rawData.powerOut;
                powerOutSquare = Math.Pow(powerOut, 2);
            }
            else
            {
                powerIn = rawData.powerIn;
                powerInSquare = Math.Pow(powerIn, 2);
            }

            bool voltageIs110V = (voltage < 180.0);

            switch (psu.GetInternalName())
            {
                case "AX1200i":
                    powerOut = (voltageIs110V ? (-5.398236E-05 * powerInSquare + 0.9791846 * powerIn - 14.75333) : (-3.724072E-05 * powerInSquare + 0.9792162 * powerIn - 13.2167));
                    break;
                case "AX860i":
                    powerOut = (voltageIs110V ? (-7.803814E-05 * powerInSquare + 0.9856293 * powerIn - 12.32274) : (-6.501415E-05 * powerInSquare + 0.9914832 * powerIn - 11.86228));
                    break;
                case "AX760i":
                    powerOut = (voltageIs110V ? (-8.497113E-05 * powerInSquare + 0.9882959 * powerIn - 12.28865) : (-6.94856E-05 * powerInSquare + 0.9947551 * powerIn - 12.14068));
                    break;
                case "HX1000i":
                    powerIn = (voltageIs110V ? (8.614363E-05 * powerOutSquare + 1.013681 * powerOut + 12.57297) : (6.625963E-05 * powerOutSquare + 1.013373 * powerOut + 11.67543));
                    break;
                case "HX850i":
                    powerIn = (voltageIs110V ? (9.402802E-05 * powerOutSquare + 1.01568 * powerOut + 12.40252) : (6.333025E-05 * powerOutSquare + 1.023295 * powerOut + 9.813914));
                    break;
                case "HX750i":
                    powerIn = (voltageIs110V ? (0.0001011842 * powerOutSquare + 1.015511 * powerOut + 12.08922) : (7.757325E-05 * powerOutSquare + 1.015346 * powerOut + 10.91073));
                    break;
                case "HX650i":
                    powerIn = (voltageIs110V ? (0.0001215054 * powerOutSquare + 1.015551 * powerOut + 9.62592) : (8.296032E-05 * powerOutSquare + 1.018958 * powerOut + 8.411756));
                    break;
                default:
                    powerOut = (voltageIs110V ? (-4.537257E-05 * powerInSquare + 0.9955244 * powerIn - 17.04104) : (-4.024153E-05 * powerInSquare + 1.002732 * powerIn - 19.12843));
                    break;
            }
            
            double efficiency = powerOut / powerIn;
            
            if (efficiency > 0.99 || efficiency < 0.4)
            {
                efficiency = Math.Min(Math.Max(efficiency, 0.4), 0.99);
                if (psuIsHX)
                    powerIn = powerOut / efficiency;
                else
                    powerOut = powerIn * efficiency;
            }

            return new PowerData(powerIn, powerOut, efficiency * 100.0);
        }
    }
}
