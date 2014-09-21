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
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED;
using CorsairLinkPlusPlus.Driver.CorsairLink.Registry;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using System;
using System.Collections.Generic;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal;
using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Node
{
    public class LinkDeviceModern : BaseLinkDevice
    {

        private volatile Dictionary<int, Cooler> coolerSensors = null;
        private volatile Dictionary<int, Thermistor> tempSensors = null;
        private volatile Dictionary<int, LED> ledSensors = null;

        private volatile int temperatureCount = -1;
        private volatile int coolerCount = -1;
        private volatile int ledCount = -1;
        private volatile byte deviceID = 0xFF;
        private volatile DeviceType actualType = DeviceType.Hub;
        private volatile string name = null;

        internal LinkDeviceModern(USB.BaseUSBDevice usbDevice, byte channel)
            : base(usbDevice, channel)
        {
            lock (subDeviceLock)
            {
                RefreshCoreValues();
            }
        }

        private void RefreshCoreValues()
        {
            int devID = GetDeviceID();
            if (devID == 0x37 || devID == 0x3A || devID == 0x3B || devID == 0x3C || devID == 0x3E || devID == 0x40 || devID == 0x41)
                actualType = DeviceType.Cooler;
            else
                actualType = DeviceType.Hub;

            switch (GetDeviceID())
            {
                case 0x37:
                    name = "Corsair H80";
                    break;
                case 0x38:
                    name = "Corsair Cooling Node";
                    break;
                case 0x39:
                    name = "Corsair Lighting Node";
                    break;
                case 0x3A:
                    name = "Corsair H100";
                    break;
                case 0x3B:
                    name = "Corsair H80i";
                    break;
                case 0x3C:
                    name = "Corsair H100i";
                    break;
                case 0x3D:
                    name = "Corsair Commander Mini";
                    break;
                case 0x3E:
                    name = "Corsair H110i";
                    break;
                case 0x3F:
                    name = "Corsair Hub";
                    break;
                case 0x40:
                    name = "Corsair H100iGT";
                    break;
                case 0x41:
                    name = "Corsair H110iGT";
                    break;
                default:
                    name = "Unknown Modern Device (0x" + string.Format("{0:x2}", GetDeviceID()) + ")";
                    break;
            }
        }

        public override DeviceType Type
        {
            get
            {
                return actualType;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        internal Thermistor GetThermistor(int idx)
        {
            DisabledCheck();

            if (tempSensors == null)
                GetSubDevicesInternal();
            return tempSensors[idx];
        }

        protected override List<IDevice> GetSubDevicesInternal()
        {
            List<IDevice> ret = base.GetSubDevicesInternal();

            lock (subDeviceLock)
            {
                // COOLERS
                if (coolerSensors == null)
                {
                    coolerSensors = new Dictionary<int, Cooler>();
                    int imax = GetCoolerCount();
                    for (int i = 0; i < imax; i++)
                        coolerSensors.Add(i, GetCooler(i));
                }

                ret.AddRange(coolerSensors.Values);

                // THERMISTORS
                if (tempSensors == null)
                {
                    tempSensors = new Dictionary<int, Thermistor>();
                    int imax = GetTemperatureCount();
                    for (int i = 0; i < imax; i++)
                        tempSensors.Add(i, new ThermistorModern(this, i));
                }

                ret.AddRange(tempSensors.Values);

                // LEDs
                if (ledSensors == null)
                {
                    ledSensors = new Dictionary<int, LED>();
                    int imax = GetLEDCount();
                    for (int i = 0; i < imax; i++)
                        ledSensors.Add(i, new LEDModern(this, i));
                }

                ret.AddRange(ledSensors.Values);
            }

            return ret;
        }

        private Cooler GetCooler(int id)
        {
            DisabledCheck();

            if (id < 0 || id >= GetCoolerCount())
                return null;
            int devID = GetDeviceID();
            if (devID == 0x3B || devID == 0x3C || devID == 0x3E || devID == 0x40 || devID == 0x41)
                if (id == GetCoolerCount() - 1)
                    return new PumpModern(this, id);
            return new FanModern(this, id);
        }

        internal void SetCurrentFan(int id, bool verify = false)
        {
            DisabledCheck();

            WriteSingleByteRegister(0x10, (byte)id, verify);
        }

        internal double GetCoolerRPM(int id)
        {
            DisabledCheck();

            byte[] ret;
            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                SetCurrentFan(id);
                ret = ReadRegister(0x16, 2);
            }
            return BitConverter.ToInt16(ret, 0);
        }

        internal void SetCurrentTemp(int id)
        {
            DisabledCheck();

            WriteSingleByteRegister(0x0C, (byte)id);
        }

        internal void SetCurrentLED(int id, bool verify = false)
        {
            DisabledCheck();

            WriteSingleByteRegister(0x04, (byte)id, verify);
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            if (!volatileOnly)
            {
                lock (subDeviceLock)
                {
                    temperatureCount = -1;
                    coolerCount = -1;
                    ledCount = -1;
                    tempSensors = null;
                    ledSensors = null;
                    coolerSensors = null;
                    deviceID = 0xFF;
                    RefreshCoreValues();
                }
            }
        }

        internal int GetCoolerCount()
        {
            DisabledCheck();

            if (coolerCount < 0)
                coolerCount = ReadSingleByteRegister(0x11);
            return coolerCount;
        }

        internal int GetTemperatureCount()
        {
            DisabledCheck();

            if (temperatureCount < 0)
                temperatureCount = ReadSingleByteRegister(0x0D);
            return temperatureCount;
        }

        internal int GetLEDCount()
        {
            DisabledCheck();

            if (ledCount < 0)
                ledCount = ReadSingleByteRegister(0x05);
            return ledCount;
        }

        public byte GetDeviceID()
        {
            DisabledCheck();

            if (deviceID == 0xFF)
                deviceID = ReadSingleByteRegister(0x00);
            return deviceID;
        }

        public int GetFirmwareVersion()
        {
            DisabledCheck();

            return BitConverter.ToInt16(ReadRegister(0x01, 2), 0);
        }
    }
}
