using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver;
using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Controller.LED;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.CLI
{
    class Program
    {
        static void PrintSensorsAndSubDevices(BaseDevice device, string prefix)
        {
            if (!device.IsValid() || !device.IsPresent())
                return;

            if (device is BaseSensorDevice)
            {
                if (device is LED)
                {
                    LED led = (LED)device;
                    ControllableSensor controllableLED = (ControllableSensor)device;

                    ControllerBase ledController = controllableLED.GetController();
                    Console.Out.WriteLine(prefix + "- " + led.GetName() + " = " + led.GetValue() + " " + led.GetUnit());
                    if (ledController != null)
                        Console.Out.WriteLine(prefix + "\t" + ((ledController == null) ? "N/A" : ledController.GetType().Name));
                    if (ledController is LEDColorCycleController)
                    {
                        //ledController = new LEDFourColorController(new Color(255, 1, 1), new Color(1, 1, 255), new Color(255, 1, 255), new Color(255, 255, 1));
                        //ledController = new LEDSingleColorController(new Color(255, 1, 1));
                        //controllableLED.SetController(ledController);
                        Console.Out.WriteLine(prefix + "\t\t" + ((LEDColorCycleController)ledController).GetColors()[0].ToString());
                    }
                }
                else if(device is Fan && device is ControllableSensor)
                {
                    Fan fan = (Fan)device;
                    ControllableSensor controllableFan = (ControllableSensor)device;

                    if (fan.GetParent().GetName().Contains("Commander Mini"))
                    {
                        controllableFan.SetController(new FanFixedPercentController(40));
                    }

                    ControllerBase fanController = controllableFan.GetController();
                    Console.Out.WriteLine(prefix + "- " + fan.GetName() + " = " + fan.GetValue() + " " + fan.GetUnit());
                    if (fanController != null)
                        Console.Out.WriteLine(prefix + "\t" + ((fanController == null) ? "N/A" : fanController.GetType().Name));
                    if(fanController is FanCurveController)
                    {
                        Console.Out.WriteLine(prefix + "\t\t" + ((FanCurveController)fanController).GetCurve().ToString().Replace("}, {", "}\r\n" + prefix + "\t\t{"));
                    }
                }
                else
                {
                    BaseSensorDevice sensor = (BaseSensorDevice)device;
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
            PrintSensorsAndSubDevices(RootDevice.GetInstance(), "");

            Console.Out.WriteLine();
            Console.Out.WriteLine("---DONE---");
            Console.In.ReadLine();
        }
    }
}
