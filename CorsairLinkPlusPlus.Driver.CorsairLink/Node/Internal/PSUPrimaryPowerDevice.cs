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
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Node.Internal
{
    class PSUPrimaryPowerDevice : BaseLinkDevice
    {
        protected readonly int id;
        protected readonly string name;
        protected readonly LinkDevicePSU psuDevice;
        protected double cachedVoltage = double.NaN;
        protected double cachedCurrent = double.NaN;
        protected double cachedPower = double.NaN;

        internal PSUPrimaryPowerDevice(LinkDevicePSU psuDevice, byte channel, int id, string name)
            : base(psuDevice, channel)
        {
            this.id = id;
            this.name = name;
            this.psuDevice = psuDevice;
        }

        public override DeviceType Type
        {
            get
            {
                return DeviceType.PSU;
            }
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            this.psuDevice.Refresh(true);
            cachedCurrent = double.NaN;
            cachedVoltage = double.NaN;
            cachedPower = double.NaN;
        }

        public override string GetLocalDeviceID()
        {
            return "PowerMain" + id;
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        protected override List<IDevice> GetSubDevicesInternal()
        {
            List<IDevice> sensors = base.GetSubDevicesInternal();
            sensors.Add(new PSUPrimaryCurrentSensor(this));
            sensors.Add(new PSUPrimaryPowerSensor(this));
            sensors.Add(new PSUPrimaryVoltageSensor(this));
            return sensors;
        }

        protected void SetPage()
        {
            psuDevice.SetMainPage(id);
        }

        protected virtual void ReadValues()
        {
            byte[] retVoltage, retCurrent, retPower;

            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                SetPage();
                retVoltage = ReadRegister(0x8B, 2);
                retCurrent = ReadRegister(0x8C, 2);
                retPower = ReadRegister(0x96, 2);
            }

            cachedVoltage = BitCodec.ToFloat(retVoltage);
            cachedCurrent = BitCodec.ToFloat(retCurrent);
            cachedPower = BitCodec.ToFloat(retPower);
        }

        internal virtual double ReadVoltage()
        {
            DisabledCheck();

            if (double.IsNaN(cachedVoltage))
                ReadValues();

            return cachedVoltage;
        }

        internal virtual double ReadCurrent()
        {
            DisabledCheck();

            if (double.IsNaN(cachedCurrent))
                ReadValues();

            return cachedCurrent;
        }

        internal virtual double ReadPower()
        {
            DisabledCheck();

            if (double.IsNaN(cachedPower))
                ReadValues();

            return cachedPower;
        }
    }
}
