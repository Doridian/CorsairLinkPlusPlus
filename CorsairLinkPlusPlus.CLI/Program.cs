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
                else if(device is CorsairFan && device is CorsairControllableSensor)
                {
                    CorsairFan fan = (CorsairFan)device;
                    CorsairControllableSensor controllableFan = (CorsairControllableSensor)device;
                    CorsairControllerBase fanController = controllableFan.GetController();
                    Console.Out.WriteLine(prefix + "- " + fan.GetName() + " = " + fan.GetValue() + " " + fan.GetUnit());
                    if (fanController != null)
                        Console.Out.WriteLine(prefix + "\t" + ((fanController == null) ? "N/A" : fanController.GetType().Name));
                    if(fanController is CorsairFanCurveController)
                    {
                        Console.Out.WriteLine(prefix + "\t\t" + ((CorsairFanCurveController)fanController).GetCurve().ToString().Replace("}, {", "}\r\n" + prefix + "\t\t{"));
                    }
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
