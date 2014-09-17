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

        internal BaseSensorDevice(BaseLinkDevice device, int id)
        {
            this.device = device;
            this.id = id;
        }

        public string GetUDID()
        {
            return device.GetUDID() + "/" + GetSensorType() + id;
        }

        public bool IsPresent()
        {
            if(cachedPresence == null)
                cachedPresence = IsPresentInternal();
            return (bool)cachedPresence;
        }

        internal virtual bool IsPresentInternal()
        {
            return true;
        }

        public virtual string GetName()
        {
            return GetSensorType() + " " + id;
        }

        public virtual void Refresh(bool volatileOnly)
        {
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

        public List<Driver.BaseDevice> GetSubDevices()
        {
            return new List<Driver.BaseDevice>();
        }

        public Driver.BaseDevice GetParent()
        {
            return device;
        }
    }
}
