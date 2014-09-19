using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common
{
    public abstract class BaseDevice : IDevice
    {
        protected readonly IDevice parent;
        protected readonly object subDeviceLock = new object();
        private volatile List<IDevice> subDevices = null;
        protected volatile bool disabled = false;

        protected BaseDevice(IDevice parent)
        {
            this.parent = parent;
        }

        public virtual bool IsPresent()
        {
            return true;
        }

        public IDevice FindBySubPath(string subPath)
        {
            int slashIdx = subPath.IndexOf('/');
            string nextPath;
            if (slashIdx < 0)
            {
                nextPath = subPath;
                subPath = null;
            }
            else
            {
                nextPath = subPath.Substring(0, slashIdx);
                subPath = subPath.Substring(slashIdx + 1);
            }

            foreach(IDevice device in GetSubDevices())
            {
                if(device.GetLocalDeviceID() == nextPath)
                {
                    if(subPath == null)
                        return device;
                    else
                        return device.FindBySubPath(subPath);
                }
            }

            return null;
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
                    device.Disable();
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
                    subDevice.Disable();

                subDevices = null;
            }
        }

        public IEnumerable<IDevice> GetSubDevices()
        {
            lock (subDeviceLock)
            {
                if (subDevices == null)
                    subDevices = GetSubDevicesInternal();
                return subDevices;
            }
        }

        protected virtual List<IDevice> GetSubDevicesInternal()
        {
            return new List<IDevice>();
        }

        public abstract string GetLocalDeviceID();

        public string GetUDID()
        {
            IDevice device = GetParent();
            if (device == null)
                return "/" + GetLocalDeviceID();
            return device.GetLocalDeviceID() + "/" + GetLocalDeviceID();
        }

        public IDevice GetParent()
        {
            return parent;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IDevice))
                return false;
            return GetUDID().Equals(((IDevice)obj).GetUDID());
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
