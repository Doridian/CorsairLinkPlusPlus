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
using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.Registry;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    class LEDModern : LED, TemperatureControllableSensor
    {
        private readonly LinkDeviceModern modernDevice;
        private byte? cachedLEDData;
        private LEDController m_controller;

        internal LEDModern(LinkDeviceModern device, int id)
            : base(device, id)
        {
            modernDevice = device;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            if (!volatileOnly)
                cachedLEDData = null;

            ((ControllerBase)Controller).Refresh(this);
        }

        internal override byte[] GetRGBInternal()
        {
            DisabledCheck();

            byte[] rgbInt;

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentLED(id);
                rgbInt = modernDevice.ReadRegister(0x07, 3);
            }

            return rgbInt;
        }

        internal override void SetRGB(byte[] rgb)
        {
            DisabledCheck();

            byte[] reg = new byte[12];
            Buffer.BlockCopy(rgb, 0, reg, 0, rgb.Length);

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentLED(id, true);
                modernDevice.WriteRegister(0x0B, reg);
            }

            Refresh(true);
        }

        public void SetTemperatureSensor(IThermistor thermistor)
        {
            DisabledCheck();

            int idx = Core.GetRelativeThermistorByte(this, thermistor);
            byte ledData = GetLEDData();
            ledData &= 0xF8; //11111000
            ledData |= (byte)idx;
            SetLEDData(ledData);
        }

        public void SetTemperature(double temperature)
        {
            DisabledCheck();

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentLED(id);
                modernDevice.WriteRegister(0x08, BitConverter.GetBytes((short)(temperature * 256.0)));
            }
        }

        public IController Controller
        {
            set
            {
                DisabledCheck();

                if (!(value is LEDController))
                    throw new ArgumentException();

                LEDController ledController = (LEDController)value;

                byte ledControllerID = ledController.GetLEDModernControllerID();

                if ((ledControllerID & 0x3F /* 00111111 */) != 0)
                    throw new ArgumentException();

                if (ledController is TemperatureDependantControllerBase)
                {
                    byte ledData = GetLEDData();
                    ledData &= 0x3F; //00111111
                    ledData |= (byte)ledControllerID;
                    SetLEDData(ledData);
                }
                else
                    SetLEDData((byte)(ledControllerID | 0x0B));

                SaveControllerData(value);
            }

            get
            {
                DisabledCheck();

                if (m_controller == null)
                    m_controller = LEDControllerRegistry.Get(this, (byte)(GetLEDData() & 0xC0 /* 11000000 */));

                return m_controller;
            }
        }

        public void SaveControllerData(IController controller)
        {
            DisabledCheck();

            if (!(controller is LEDController))
                throw new ArgumentException();

            ((ControllerBase)Controller).Apply(this);
        }

        private void SetLEDData(byte ledData)
        {
            DisabledCheck();

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentLED(id, true);
                modernDevice.WriteSingleByteRegister(0x06, ledData);
                cachedLEDData = null;
            }
        }

        private byte GetLEDData()
        {
            DisabledCheck();

            if (cachedLEDData == null)
            {
                using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
                {
                    modernDevice.SetCurrentLED(id);
                    cachedLEDData = modernDevice.ReadSingleByteRegister(0x06);
                }
            }
            return (byte)cachedLEDData;
        }

        public IThermistor GetTemperatureSensor()
        {
            DisabledCheck();

            int idx = (GetLEDData() & 0x07 /* 00000111 */);
            if (idx == 7)
                return null;
            return modernDevice.GetThermistor(idx);
        }

        internal override byte[] GetFixedRGBCycleColors()
        {
            DisabledCheck();

            byte[] colors;

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentLED(id);
                colors = modernDevice.ReadRegister(0x0B, 12);
            }

            return colors;
        }

        internal override void SetFixedRGBCycleColors(byte[] colors)
        {
            DisabledCheck();

            if(colors.Length > 12)
                throw new ArgumentException();

            if (colors.Length < 12)
            {
                byte[] newColors = new byte[12];
                Buffer.BlockCopy(colors, 0, newColors, 0, colors.Length);
                colors = newColors;
            }

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentLED(id, true);
                modernDevice.WriteRegister(0x0B, colors, true);
            }
        }

        internal override ControlCurve<double, Color> GetControlCurve()
        {
            byte[] tempTable, colorTable;

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentLED(id);
                tempTable = modernDevice.ReadRegister(0x09, 6);
                colorTable = modernDevice.ReadRegister(0x0A, 9);
            }

            List<CurvePoint<double, Color>> points = new List<CurvePoint<double, Color>>();

            for (int i = 0; i < 3; i++)
            {
                int j = i * 3;
                points.Add(new CurvePoint<double, Color>(BitConverter.ToInt16(tempTable, i * 2) / 256.0, new Color(colorTable[j], colorTable[j + 1], colorTable[j + 2])));
            }

            return new ControlCurve<double, Color>(points);
        }

        internal override void SetControlCurve(ControlCurve<double, Color> curve)
        {
            List<CurvePoint<double, Color>> points = curve.Points;
            if (points.Count != 3)
                throw new ArgumentException();

            byte[] tempTable = new byte[6];
            byte[] colorTable = new byte[9];

            for(int i = 0; i < 3; i++)
            {
                CurvePoint<double, Color> point = points[i];
                Buffer.BlockCopy(BitConverter.GetBytes((short)(point.X * 256)), 0, tempTable, i, 2);
                Buffer.BlockCopy(point.Y.ToArray(), 0, colorTable, i, 2);
            }

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentLED(id, true);
                modernDevice.WriteRegister(0x09, tempTable, true);
                modernDevice.WriteRegister(0x0A, colorTable, true);
            }
        }

        public IEnumerable<RegisteredController> GetValidControllers()
        {
            return new RegisteredController[] {
                ControllerRegistry.Get("CorsairLink.LEDSingleColorController"),
                ControllerRegistry.Get("CorsairLink.LEDTwoColorController"),
                ControllerRegistry.Get("CorsairLink.LEDFourColorController"),
                ControllerRegistry.Get("CorsairLink.LEDTemperatureController"),
            };
        }

        public IEnumerable<string> ValidControllerNames
        {
            get
            {
                return new string[] {
                    "CorsairLink.LEDSingleColorController",
                    "CorsairLink.LEDTwoColorController",
                    "CorsairLink.LEDFourColorController",
                    "CorsairLink.LEDTemperatureController",
                };
            }
        }
    }
}
