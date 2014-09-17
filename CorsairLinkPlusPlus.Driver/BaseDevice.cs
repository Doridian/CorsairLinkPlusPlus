using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver
{
    public abstract class BaseDevice
    {
        protected readonly BaseDevice parent;

        internal BaseDevice(BaseDevice parent)
        {
            this.parent = parent;
        }

        public virtual bool IsPresent()
        {
            return true;
        }

        public abstract string GetName();

        public virtual void Refresh(bool volatileOnly)
        {

        }

        public abstract List<BaseDevice> GetSubDevices();

        public abstract string GetUDID();

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
