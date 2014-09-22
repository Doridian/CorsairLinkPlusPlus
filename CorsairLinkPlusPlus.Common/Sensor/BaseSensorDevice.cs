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
using CorsairLinkPlusPlus.Common.Utility;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public abstract class BaseSensorDevice : BaseDevice, ISensor
    {
        protected object cachedValue = null;
        protected bool? cachedPresence = null;

        protected BaseSensorDevice(IDevice parent) : base(parent) { }

        public override bool Present
        {
            get
            {
                if (cachedPresence == null)
                    cachedPresence = IsPresentInternal();
                return (bool)cachedPresence;
            }
        }

        protected virtual bool IsPresentInternal()
        {
            return true;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            cachedValue = null;
            if (!volatileOnly)
                cachedPresence = null;
        }

        public object Value
        {
            get
            {
                if (cachedValue == null)
                    cachedValue = GetValueInternal();
                return cachedValue;
            }
        }

        public abstract SensorType SensorType { get; }

        protected abstract object GetValueInternal();

        public abstract Unit Unit { get; }
    }
}
