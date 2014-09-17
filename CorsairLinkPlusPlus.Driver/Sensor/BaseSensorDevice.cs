using CorsairLinkPlusPlus.Driver.Node;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class BaseSensorDevice : Driver.BaseDevice
    {
        internal readonly BaseLinkDevice device;
        internal readonly int id;
        protected double cachedValue = double.NaN;
        protected bool? cachedPresence = null;

        internal BaseSensorDevice(BaseLinkDevice device, int id) : base(device)
        {
            this.device = device;
            this.id = id;
        }

        public override string GetLocalDeviceID()
        {
            return "Sensor" + GetSensorType() + id;
        }

        public override bool IsPresent()
        {
            if(cachedPresence == null)
                cachedPresence = IsPresentInternal();
            return (bool)cachedPresence;
        }

        internal virtual bool IsPresentInternal()
        {
            return true;
        }

        public override string GetName()
        {
            return GetSensorType() + " " + id;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            device.Refresh(volatileOnly);
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

        internal abstract double GetValueInternal();

        public abstract string GetUnit();
    }
}
