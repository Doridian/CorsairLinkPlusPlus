using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Common.Utility
{
    public enum Unit {
        DegreeCelsius,
        Ampere,
        Volt,
        Watt,
        RPM,
        Percent,
        Color,
        Unknown
    }

    public static class UnitExtensions
    {
        public static string GetPostfix(this Unit unit, bool longFormat = false)
        {
            switch (unit)
            {
                case Unit.DegreeCelsius:
                    return longFormat ? "Degrees Celsius" : "°C";
                case Unit.Ampere:
                    return longFormat ? "Ampere" : "A";
                case Unit.Volt:
                    return longFormat ? "Volt" : "V";
                case Unit.Watt:
                    return longFormat ? "Watt" : "W";
                case Unit.RPM:
                    return longFormat ? "Rounds per Minute" : "RPM";
                case Unit.Percent:
                    return longFormat ? "Percent" : "%";
                case Unit.Color:
                    return longFormat ? "Color" : "RGB";

            }
            return longFormat ? "Unknown" : "N/A";
        }
    }
}
