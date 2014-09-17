using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver
{
    public interface BaseDevice
    {
        bool IsPresent();
        string GetName();
        void Refresh(bool volatileOnly);
        List<BaseDevice> GetSubDevices();
        string GetUDID();
        BaseDevice GetParent();
    }
}
