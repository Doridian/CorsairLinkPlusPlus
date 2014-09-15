using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver
{
    public interface CorsairBaseDevice
    {
        bool IsPresent();
        string GetName();
        void Refresh();
        List<CorsairBaseDevice> GetSubDevices();
        string GetUDID();
    }
}
