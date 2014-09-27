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

using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.Registry;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    class FanModern : Fan, TemperatureControllableSensor
    {
        private readonly LinkDeviceModern modernDevice;
        protected byte? cachedFanData = null;
        private IFanController m_controller = null;

        internal FanModern(LinkDeviceModern device, int id)
            : base(device, id)
        {
            modernDevice = device;
        }

        private byte GetFanData()
        {
            DisabledCheck();

            if (cachedFanData == null)
            {
                using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
                {
                    modernDevice.SetCurrentFan(id);
                    cachedFanData = modernDevice.ReadSingleByteRegister(0x12);
                }
            }

            return (byte)cachedFanData;
        }

        private void SetFanData(byte fanData)
        {
            DisabledCheck();

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteSingleByteRegister(0x12, fanData, true);
                cachedFanData = null;
            }
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            if (!volatileOnly)
                cachedFanData = null;
            ((ControllerBase)Controller).Refresh(this);
        }

        protected override object GetValueInternal()
        {
            DisabledCheck();
            return modernDevice.GetCoolerRPM(id);
        }

        private bool FanDataBitSet(int bit)
        {
            bit = 1 << bit;
            return (GetFanData() & bit) == bit;
        }

        protected override bool IsPresentInternal()
        {
            return FanDataBitSet(7);
        }

        internal override bool IsPWMInternal()
        {
            return FanDataBitSet(0);
        }

        public void SetTemperatureSensor(IThermistor thermistor)
        {
            DisabledCheck();

            int idx = CoreUtil.GetRelativeThermistorByte(this, thermistor);
            byte fanData = GetFanData();
            fanData &= 0x8F; //10001111
            fanData |= (byte)(idx << 4);
            SetFanData(fanData);
        }

        public void SetTemperature(double temperature)
        {
            DisabledCheck();

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteRegister(0x15, BitConverter.GetBytes((short)(temperature * 256.0)), true);
            }
        }

        public IController Controller 
        {
            set
            {
                DisabledCheck();

                if (!(value is IFanController))
                    throw new ArgumentException();

                IFanController fanController = (IFanController)value;

                byte fanControllerID = fanController.GetFanModernControllerID();
                if ((fanControllerID & 0xF1) != 0)
                    throw new ArgumentException();

                m_controller = fanController;

                byte fanData = GetFanData();
                fanData &= 0xF1; //11110001
                fanData |= (byte)fanControllerID;
                SetFanData(fanData);

                SaveControllerData(value);
            }

            get
            {
                DisabledCheck();

                if (m_controller == null)
                {
                    m_controller = FanControllerRegistry.Get(this, (byte)(GetFanData() & 0x0E /*00001110*/));
                }
                return m_controller;
            }
        }

        public void SaveControllerData(IController controller)
        {
            DisabledCheck();

            if (!(controller is IFanController))
                throw new ArgumentException();
            ((ControllerBase)controller).Apply(this);
        }

        internal override void SetFixedRPM(double fixedRPM)
        {
            DisabledCheck();

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteRegister(0x14, BitConverter.GetBytes((short)fixedRPM), true);
            }
        }

        internal override void SetFixedPercent(double percent)
        {
            DisabledCheck();

            byte percentB = (byte)(percent * 2.55);

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteSingleByteRegister(0x13, percentB, true);
            }
        }

        internal override double GetFixedPercent()
        {
            DisabledCheck();

            byte percentB;

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id);
                percentB = modernDevice.ReadSingleByteRegister(0x13);
            }

            return (short)(percentB / 2.55);
        }

        internal override double GetFixedRPM()
        {
            DisabledCheck();

            byte[] rpmB;

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id);
                rpmB = modernDevice.ReadRegister(0x14, 2);
            }

            return BitConverter.ToInt16(rpmB, 0);
        }

        internal override ControlCurve<double, double> GetControlCurve()
        {
            DisabledCheck();

            byte[] tempTable, rpmTable;

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id);
                tempTable = modernDevice.ReadRegister(0x1A, 10);
                rpmTable = modernDevice.ReadRegister(0x19, 10);
            }

            List<CurvePoint<double, double>> points = new List<CurvePoint<double, double>>();

            for (int i = 0; i < 10; i += 2)
                points.Add(new CurvePoint<double, double>(BitConverter.ToInt16(tempTable, i) / 256, BitConverter.ToInt16(rpmTable, i)));

            return new ControlCurve<double, double>(points);
        }

        internal override void SetControlCurve(ControlCurve<double, double> curve)
        {
            DisabledCheck();

            List<CurvePoint<double, double>> points = curve.Points;
            if (points.Count != 5)
                throw new ArgumentException();

            byte[] tempTable = new byte[10];
            byte[] rpmTable = new byte[10];

            for (int i = 0; i < 10; i += 2)
            {
                CurvePoint<double, double> point = points[i / 2];
                Buffer.BlockCopy(BitConverter.GetBytes((short)(point.X * 256)), 0, tempTable, i, 2);
                Buffer.BlockCopy(BitConverter.GetBytes((short)point.Y), 0, rpmTable, i, 2);
            }

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteRegister(0x1A, tempTable, true);
                modernDevice.WriteRegister(0x19, rpmTable, true);
            }
        }

        public IThermistor GetTemperatureSensor()
        {
            DisabledCheck();

            int idx = (GetFanData() & 0x70 /* 01110000 */) >> 4;
            if (idx == 7)
                return null;
            return modernDevice.GetThermistor(idx);
        }

        internal override void SetMinimalRPM(double rpm)
        {
            DisabledCheck();

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteRegister(0x18, BitConverter.GetBytes((short)rpm));
            }
        }

        internal override double GetMinimalRPM()
        {
            DisabledCheck();

            short minRPM;

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentFan(id);
                minRPM = BitConverter.ToInt16(modernDevice.ReadRegister(0x18, 2), 0);
            }

            return minRPM;
        }

        public IEnumerable<string> ValidControllerNames
        {
            get
            {
                return new string[] {
                    "Fan.CorsairLink.Default",
                    "Fan.CorsairLink.BalancedMode",
                    "Fan.CorsairLink.QuiteMode",
                    "Fan.CorsairLink.PerformanceMode",
                    "Fan.CorsairLink.CustomCurve",
                    "Fan.CorsairLink.FixedRPM",
                    "Fan.CorsairLink.FixedPercent",
                };
            }
        }
    }
}
