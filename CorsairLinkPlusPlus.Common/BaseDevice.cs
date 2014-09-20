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

        public virtual bool Present
        {
            get
            {
                return true;
            }
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
                    if(string.IsNullOrEmpty(subPath))
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

        public virtual bool Valid
        {
            get
            {
                return !disabled;
            }
        }

        public abstract string Name { get; }

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

        public virtual IEnumerable<IDevice> GetSubDevices()
        {
            List<IDevice> ret = new List<IDevice>();
            lock (subDeviceLock)
            {
                if (subDevices == null)
                    subDevices = GetSubDevicesInternal();
            }
            foreach (IDevice device in subDevices)
                if (device.Present && device.Valid)
                    ret.Add(device);
            return ret;
        }

        protected virtual List<IDevice> GetSubDevicesInternal()
        {
            return new List<IDevice>();
        }

        public abstract string GetLocalDeviceID();

        public string AbsolutePath
        {
            get
            {
                if (parent == null)
                    return "/" + GetLocalDeviceID();
                string path = parent.AbsolutePath;
                if (path.LastIndexOf('/') == path.Length - 1)
                    return path + GetLocalDeviceID();
                return path + '/' + GetLocalDeviceID();
            }
        }

        public IDevice GetParent()
        {
            return parent;
        }

        public string ParentPath
        {
            get
            {
                return (parent == null) ? null : parent.AbsolutePath;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IDevice))
                return false;
            return AbsolutePath.Equals(((IDevice)obj).AbsolutePath);
        }

        public override int GetHashCode()
        {
            return AbsolutePath.GetHashCode();
        }

        public override string ToString()
        {
            return Name + " [" + AbsolutePath + "]";
        }

        public IEnumerable<string> ChildrenPaths
        {
            get
            {
                List<string> ret = new List<string>();
                foreach(IDevice device in GetSubDevices())
                {
                    ret.Add(device.AbsolutePath);
                }
                return ret;
            }
        }
    }
}
