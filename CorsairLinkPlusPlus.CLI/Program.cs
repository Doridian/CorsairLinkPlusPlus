using CorsairLinkPlusPlus.Driver;
using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.CLI
{
    class Program
    {
        static void PrintSensorsAndSubDevices(CorsairBaseDevice device, string prefix)
        {
            if (device is CorsairSensor)
            {
                if (device is CorsairLED)
                {
                    CorsairLED led = (CorsairLED)device;
                    Console.Out.WriteLine(prefix + "- " + led.GetName() + " = " + led.GetColor().ToString());
                }
                else if(device is CorsairFan && device is CorsairTemperatureControllableSensor)
                {
                    CorsairFan fan = (CorsairFan)device;
                    CorsairTemperatureControllableSensor controllableFan = (CorsairTemperatureControllableSensor)device;
                    CorsairControllerBase fanController = controllableFan.GetController();
                    Console.Out.Write(prefix + "- " + fan.GetName() + " = " + fan.GetValue() + " " + fan.GetUnit() + ((fanController == null) ? "N/A" : fanController.GetType().Name));
                    if(fanController is CorsairFanCurveController)
                    {
                        Console.Out.Write(((CorsairFanCurveController)fanController).GetCurve());
                    }
                    Console.Out.WriteLine();
                }
                else
                {
                    CorsairSensor sensor = (CorsairSensor)device;
                    Console.Out.WriteLine(prefix + "- " + sensor.GetName() + " = " + sensor.GetValue() + " " + sensor.GetUnit());
                }
            }
            else
            {
                Console.Out.WriteLine(prefix + "+ " + device.GetName());

                foreach (CorsairBaseDevice subDevice in device.GetSubDevices())
                {
                    PrintSensorsAndSubDevices(subDevice, prefix + "\t");
                }
            }
        }

        static void Main(string[] args)
        {
            CorsairLinkDeviceEnumerator enumerator = new CorsairLinkDeviceEnumerator();
            List<CorsairLinkUSBDevice> usbDevices = enumerator.GetDevices();

            foreach (CorsairLinkUSBDevice usbDevice in usbDevices)
            {
                PrintSensorsAndSubDevices(usbDevice, "");
            }

            Console.Out.WriteLine();
            Console.Out.WriteLine("---DONE---");
            Console.In.ReadLine();
        }
    }
}
