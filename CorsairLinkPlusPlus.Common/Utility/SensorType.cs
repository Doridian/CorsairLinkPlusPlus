using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Common.Utility
{
    public enum SensorType
    {
        Voltage,
        Usage,
        Temperature,
        Pump,
        LED,
        Fan,
        Current,
        Efficiency,
        Power
    }

    public static class SensorTypeExtensions
    {
        public static string GetName(this SensorType unit, bool longFormat = false)
        {
            switch (unit)
            {
                case SensorType.Voltage:
                    return longFormat ? "Voltage" : "Voltage";
                case SensorType.Usage:
                    return longFormat ? "Usage" : "Usage";
                case SensorType.Temperature:
                    return longFormat ? "Temperature" : "Temp";
                case SensorType.Pump:
                    return longFormat ? "Pump" : "Pump";
                case SensorType.LED:
                    return longFormat ? "LED" : "LED";
                case SensorType.Fan:
                    return longFormat ? "Fan" : "Fan";
                case SensorType.Current:
                    return longFormat ? "Current" : "PWMFan";
                case SensorType.Efficiency:
                    return longFormat ? "Efficiency" : "Eff.";
                case SensorType.Power:
                    return longFormat ? "Power" : "Power";

            }
            return "N/A";
        }

        public static Unit GetAssosiatedUnit(this SensorType unit)
        {
            switch (unit)
            {
                case SensorType.Voltage:
                    return Unit.Volt;
                case SensorType.Usage:
                    return Unit.Percent;
                case SensorType.Temperature:
                    return Unit.DegreeCelsius;
                case SensorType.Pump:
                    return Unit.RPM;
                case SensorType.LED:
                    return Unit.Color;
                case SensorType.Fan:
                    return Unit.RPM;
                case SensorType.Current:
                    return Unit.Ampere;
                case SensorType.Efficiency:
                    return Unit.Percent;
                case SensorType.Power:
                    return Unit.Watt;
            }
            return Unit.Unknown;
        }
    }
}
