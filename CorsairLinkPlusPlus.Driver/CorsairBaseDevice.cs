using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver
{
    public interface CorsairBaseDevice
    {
        bool IsPresent();
        string GetName();
        void Refresh(bool volatileOnly);
        List<CorsairBaseDevice> GetSubDevices();
        string GetUDID();
        CorsairBaseDevice GetParent();
    }
}
