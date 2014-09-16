using CorsairLinkPlusPlus.Driver;
using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    if (fan.GetUDID().EndsWith("5"))
                    {
                        //controllableFan.SetController(new CorsairFanFixedRPMController(2000));
                        //controllableFan.SetController(new CorsairFanFixedPercentController(40));
                    }
                    Console.Out.WriteLine(prefix + "- " + fan.GetName() + " = " + fan.GetValue() + " " + fan.GetUnit());
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
