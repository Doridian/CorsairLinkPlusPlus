using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver
{
    public interface CorsairBaseDevice
    {
        bool IsPresent();
        string GetName();
        List<CorsairBaseDevice> GetSubDevices();
    }
}
