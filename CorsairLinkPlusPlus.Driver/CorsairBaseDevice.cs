using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver
{
    public interface CorsairBaseDevice
    {
        bool IsPresent();
        string GetName();
        List<CorsairBaseDevice> GetSubDevices();
    }
}
