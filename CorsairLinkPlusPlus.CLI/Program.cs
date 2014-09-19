using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Sensor;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.CLI
{
    class Program
    {
        static void PrintSensorsAndSubDevices(IDevice device, string prefix)
        {
            if (!device.IsValid() || !device.IsPresent())
                return;

            if (device is ISensor)
            {
                if (device is ILED)
                {
                    ILED led = (ILED)device;
                    IControllableSensor controllableLED = (IControllableSensor)device;

                    IController ledController = controllableLED.GetController();
                    Console.Out.WriteLine(prefix + "- " + led.GetName() + " = " + led.GetValue() + " " + led.GetUnit());
                    if (ledController != null)
                        Console.Out.WriteLine(prefix + "\t" + ((ledController == null) ? "N/A" : ledController.GetType().Name));
                    if (ledController is IFixedColorCycleController)
                    {
                        //ledController = new LEDFourColorController(new Color(255, 1, 1), new Color(1, 1, 255), new Color(255, 1, 255), new Color(255, 255, 1));
                        //ledController = new LEDSingleColorController(new Color(255, 1, 1));
                        //controllableLED.SetController(ledController);
                        Console.Out.WriteLine(prefix + "\t\t" + ((IFixedColorCycleController)ledController).GetCycle()[0].ToString());
                    }
                }
                else if(device is IFan && device is IControllableSensor)
                {
                    IFan fan = (IFan)device;
                    IControllableSensor controllableFan = (IControllableSensor)device;

                    if (fan.GetParent().GetName().Contains("Commander Mini"))
                    {
                        controllableFan.SetController(new CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan.FanFixedPercentController(40));
                    }

                    IController fanController = controllableFan.GetController();
                    Console.Out.WriteLine(prefix + "- " + fan.GetName() + " = " + fan.GetValue() + " " + fan.GetUnit());
                    if (fanController != null)
                        Console.Out.WriteLine(prefix + "\t" + ((fanController == null) ? "N/A" : fanController.GetType().Name));
                    if(fanController is ICurveNumberController)
                    {
                        Console.Out.WriteLine(prefix + "\t\t" + ((ICurveNumberController)fanController).GetCurve().ToString().Replace("}, {", "}\r\n" + prefix + "\t\t{"));
                    }
                }
                else
                {
                    ISensor sensor = (ISensor)device;
                    Console.Out.WriteLine(prefix + "- " + sensor.GetName() + " = " + sensor.GetValue() + " " + sensor.GetUnit());
                }
            }
            else
            {
                Console.Out.WriteLine(prefix + "+ " + device.GetName());

                foreach (BaseDevice subDevice in device.GetSubDevices())
                {
                    PrintSensorsAndSubDevices(subDevice, prefix + "\t");
                }
            }
        }

        static void Main(string[] args)
        {
            PrintSensorsAndSubDevices(CorsairLinkPlusPlus.Driver.CorsairLink.USB.RootDevice.GetInstance(), "");

            Console.Out.WriteLine();
            Console.Out.WriteLine("---DONE---");
            Console.In.ReadLine();
        }
    }
}
