using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Node.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Utility
{
    class PowerCurves
    {
        internal static double Interpolate(ref double powerOut, ref double powerIn, double voltage, LinkDevicePSU psu)
        {
            bool psuIsHX = psu is LinkDevicePSUHX;
            double _powerOut = 0.0;
            double _powerOutSquare = 0.0;
            double _powerIn = 0.0;
            double _powerInSquare = 0.0;
            if (psuIsHX)
            {
                _powerOut = powerOut;
                _powerOutSquare = _powerOut * _powerOut;
            }
            else
            {
                _powerIn = powerIn;
                _powerInSquare = _powerIn * _powerIn;
            }

            switch (psu.GetInternalName())
            {
                case "AX1200i":
                    _powerOut = ((voltage < 180.0) ? (-5.398236E-05 * _powerInSquare + 0.9791846 * _powerIn - 14.75333) : (-3.724072E-05 * _powerInSquare + 0.9792162 * _powerIn - 13.2167));
                    break;
                case "AX860i":
                    _powerOut = ((voltage < 180.0) ? (-7.803814E-05 * _powerInSquare + 0.9856293 * _powerIn - 12.32274) : (-6.501415E-05 * _powerInSquare + 0.9914832 * _powerIn - 11.86228));
                    break;
                case "AX760i":
                    _powerOut = ((voltage < 180.0) ? (-8.497113E-05 * _powerInSquare + 0.9882959 * _powerIn - 12.28865) : (-6.94856E-05 * _powerInSquare + 0.9947551 * _powerIn - 12.14068));
                    break;
                case "HX1000i":
                    _powerIn = ((voltage < 180.0) ? (8.614363E-05 * _powerOutSquare + 1.013681 * _powerOut + 12.57297) : (6.625963E-05 * _powerOutSquare + 1.013373 * _powerOut + 11.67543));
                    break;
                case "HX850i":
                    _powerIn = ((voltage < 180.0) ? (9.402802E-05 * _powerOutSquare + 1.01568 * _powerOut + 12.40252) : (6.333025E-05 * _powerOutSquare + 1.023295 * _powerOut + 9.813914));
                    break;
                case "HX750i":
                    _powerIn = ((voltage < 180.0) ? (0.0001011842 * _powerOutSquare + 1.015511 * _powerOut + 12.08922) : (7.757325E-05 * _powerOutSquare + 1.015346 * _powerOut + 10.91073));
                    break;
                case "HX650i":
                    _powerIn = ((voltage < 180.0) ? (0.0001215054 * _powerOutSquare + 1.015551 * _powerOut + 9.62592) : (8.296032E-05 * _powerOutSquare + 1.018958 * _powerOut + 8.411756));
                    break;
                default:
                    _powerOut = ((voltage < 180.0) ? (-4.537257E-05 * _powerInSquare + 0.9955244 * _powerIn - 17.04104) : (-4.024153E-05 * _powerInSquare + 1.002732 * _powerIn - 19.12843));
                    break;
            }
            
            double _efficiency = _powerOut / _powerIn;
            
            bool _efficiencyOutOfRange = false;
            if (_efficiency > 0.99)
            {
                _efficiency = 0.99;
                _efficiencyOutOfRange = true;
            }
            else if (_efficiency < 0.4)
            {
                _efficiency = 0.4;
                _efficiencyOutOfRange = true;
            }

            if (_efficiencyOutOfRange)
            {
                if (psuIsHX)
                {
                    _powerIn = _powerOut / _efficiency;
                }
                else
                {
                    _powerOut = _powerIn * _efficiency;
                }
            }

            powerOut = _powerOut;
            powerIn = _powerIn;
            return _efficiency * 100.0;
        }
    }
}
