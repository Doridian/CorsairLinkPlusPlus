using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Common
{
    public abstract class BaseDevice
    {
        protected readonly BaseDevice parent;
        protected readonly object subDeviceLock = new object();
        private volatile List<BaseDevice> subDevices = null;
        protected volatile bool disabled = false;

        protected BaseDevice(BaseDevice parent)
        {
            this.parent = parent;
        }

        public virtual bool IsPresent()
        {
            return true;
        }

        protected void DisabledCheck()
        {
            if (disabled)
                throw new InvalidOperationException();
        }

        public void Disable()
        {
            lock (subDeviceLock)
            {
                disabled = true;
                foreach (BaseDevice device in GetSubDevices())
                {
                    device.Disable();
                }
            }
        }

        public bool IsValid()
        {
            return !disabled;
        }

        public abstract string GetName();

        public virtual void Refresh(bool volatileOnly)
        {
            DisabledCheck();

            if (volatileOnly)
                return;

            lock (subDeviceLock)
            {
                foreach (BaseDevice subDevice in subDevices)
                {
                    subDevice.Disable();
                }

                subDevices = null;
            }
        }

        public List<BaseDevice> GetSubDevices()
        {
            lock (subDeviceLock)
            {
                if (subDevices == null)
                    subDevices = GetSubDevicesInternal();
                return subDevices;
            }
        }

        protected virtual List<BaseDevice> GetSubDevicesInternal()
        {
            return new List<BaseDevice>();
        }

        public abstract string GetLocalDeviceID();

        public string GetUDID()
        {
            BaseDevice device = GetParent();
            if (device == null)
                return "/" + GetLocalDeviceID();
            return device.GetLocalDeviceID() + "/" + GetLocalDeviceID();
        }

        public BaseDevice GetParent()
        {
            return parent;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseDevice))
                return false;
            return GetUDID().Equals(((BaseDevice)obj).GetUDID());
        }

        public override int GetHashCode()
        {
            return GetUDID().GetHashCode();
        }

        public override string ToString()
        {
            return GetName() + " [" + GetUDID() + "]";
        }
    }
}
