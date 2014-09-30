#region LICENSE
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
 #endregion

using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using Microsoft.Win32;
using System;
using System.Threading;
using System.Linq;

namespace CorsairLinkPlusPlus.CLI
{
    class Program
    {
        private static void FixUpRegistry()
        {
            bool flag = false;
            try
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Enum\\USB\\VID_1B1C&PID_0C04");
                string[] subKeyNames = registryKey.GetSubKeyNames();
                for (int i = 0; i < subKeyNames.Length; i++)
                {
                    string str = subKeyNames[i];
                    RegistryKey registryKey2 = registryKey.OpenSubKey(str + "\\Device Parameters", true);
                    flag |= ((int)registryKey2.GetValue("DeviceSelectiveSuspended", 0) != 0);
                    flag |= ((int)registryKey2.GetValue("EnhancedPowerManagementEnabled", 0) != 0);
                    flag |= ((int)registryKey2.GetValue("SelectiveSuspendEnabled", 0) != 0);
                    registryKey2.SetValue("DeviceSelectiveSuspended", 0);
                    registryKey2.SetValue("SelectiveSuspendEnabled", 0);
                    registryKey2.SetValue("EnhancedPowerManagementEnabled", 0);
                    if (registryKey2.GetSubKeyNames().Contains("AllowIdleIrpInD3"))
                    {
                        if (registryKey2.GetValueKind("AllowIdleIrpInD3") == RegistryValueKind.DWord)
                        {
                            flag |= ((int)registryKey2.GetValue("AllowIdleIrpInD3", 0) != 0);
                            registryKey2.SetValue("AllowIdleIrpInD3", 0);
                        }
                        else
                        {
                            if (registryKey2.GetValueKind("AllowIdleIrpInD3") == RegistryValueKind.Binary)
                            {
                                bool arg_176_0 = flag;
                                RegistryKey arg_164_0 = registryKey2;
                                string arg_164_1 = "AllowIdleIrpInD3";
                                byte[] array = new byte[1];
                                flag = (arg_176_0 | ((byte[])arg_164_0.GetValue(arg_164_1, array))[0] != 0);
                                RegistryKey arg_188_0 = registryKey2;
                                string arg_188_1 = "AllowIdleIrpInD3";
                                array = new byte[1];
                                arg_188_0.SetValue(arg_188_1, array);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

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

                    {
                        IFixedColorController _ledController = (IFixedColorController)ControllerRegistry.Get("LED.CorsairLink.SingleColor").New();
                        _ledController.Value = new Color(255, 1, 1);
                        controllableLED.Controller = _ledController;
                    }

                    IController ledController = controllableLED.Controller;
                    Console.Out.WriteLine(prefix + "- " + led.Name + " = " + led.Value + " " + led.Unit.GetPostfix());
                    if (ledController != null)
                        Console.Out.WriteLine(prefix + "\t" + ((ledController == null) ? "N/A" : ledController.Name));
                    if (ledController is IFixedColorCycleController)
                    {
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
                        Console.Out.WriteLine(prefix + "\t" + ((fanController == null) ? "N/A" : fanController.Name));
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
            FixUpRegistry();

            while (true)
            {
                Console.Clear();
                Console.Out.WriteLine("--START--");
                PrintSensorsAndSubDevices(RootDevice.GetInstance(), "");
                Console.Out.WriteLine("-- END --");
                Thread.Sleep(1000);
            }
        }
    }
}
