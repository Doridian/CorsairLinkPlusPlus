using CorsairLinkPlusPlus.Common;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public abstract class BaseSensorDevice : BaseDevice, ISensor
    {
        protected double cachedValue = double.NaN;
        protected bool? cachedPresence = null;

        protected BaseSensorDevice(IDevice parent) : base(parent) { }

        public override bool IsPresent()
        {
            if (cachedPresence == null)
                cachedPresence = IsPresentInternal();
            return (bool)cachedPresence;
        }

        protected virtual bool IsPresentInternal()
        {
            return true;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            cachedValue = double.NaN;
            if (!volatileOnly)
                cachedPresence = null;
        }

        public double GetValue()
        {
            if (double.IsNaN(cachedValue))
                cachedValue = GetValueInternal();
            return cachedValue;
        }

        public abstract string GetSensorType();

        protected abstract double GetValueInternal();

        public abstract string GetUnit();
    }
}
