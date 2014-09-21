/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CorsairLinkPlusPlus.CLI
{
    class Program
    {
        static void PrintSensorsAndSubDevices(IDevice device, string prefix)
        {
            if (!device.Valid || !device.Present)
                return;

            device.Refresh(true);

            if (device is ISensor)
            {
                if (device is ILED)
                {
                    ILED led = (ILED)device;
                    IControllableSensor controllableLED = (IControllableSensor)device;

                    IController ledController = controllableLED.Controller;
                    Console.Out.WriteLine(prefix + "- " + led.Name + " = " + led.Value + " " + led.Unit.GetPostfix());
                    if (ledController != null)
                        Console.Out.WriteLine(prefix + "\t" + ((ledController == null) ? "N/A" : ledController.GetType().Name));
                    if (ledController is IFixedColorCycleController)
                    {
                        //ledController = new LEDFourColorController(new Color(255, 1, 1), new Color(1, 1, 255), new Color(255, 1, 255), new Color(255, 255, 1));
                        //ledController = new LEDSingleColorController(new Color(255, 1, 1));
                        //controllableLED.SetController(ledController);
                        Console.Out.WriteLine(prefix + "\t\t" + ((IFixedColorCycleController)ledController).Value[0].ToString());
                    }
                }
                else if(device is IFan && device is IControllableSensor)
                {
                    IFan fan = (IFan)device;
                    IControllableSensor controllableFan = (IControllableSensor)device;

                    /*if (fan.GetParent().Name.Contains("Commander Mini"))
                    {
                        IFixedNumberController _fanController = (IFixedNumberController)ControllerRegistry.Get("CorsairLink.FanFixedPercentController").New();
                        _fanController.Value = 40;
                        controllableFan.Controller = _fanController;
                    }*/

                    IController fanController = controllableFan.Controller;
                    Console.Out.WriteLine(prefix + "- " + fan.Name + " = " + fan.Value + " " + fan.Unit.GetPostfix());
                    if (fanController != null)
                        Console.Out.WriteLine(prefix + "\t" + ((fanController == null) ? "N/A" : fanController.GetType().Name));
                    if(fanController is ICurveNumberController)
                    {
                        Console.Out.WriteLine(prefix + "\t\t" + ((ICurveNumberController)fanController).Value.ToString().Replace("}, {", "}\r\n" + prefix + "\t\t{"));
                    }
                }
                else
                {
                    ISensor sensor = (ISensor)device;
                    Console.Out.WriteLine(prefix + "- " + sensor.Name + " = " + sensor.Value + " " + sensor.Unit.GetPostfix());
                }
            }
            else
            {
                Console.Out.WriteLine(prefix + "+ " + device.Name);

                foreach (BaseDevice subDevice in device.GetSubDevices())
                {
                    PrintSensorsAndSubDevices(subDevice, prefix + "\t");
                }
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                PrintSensorsAndSubDevices(RootDevice.GetInstance(), "");
                Thread.Sleep(1000);
            }
        }
    }
}
