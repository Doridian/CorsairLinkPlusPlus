using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common
{
    public interface IDevice
    {
        bool IsPresent();
        void Disable();
        bool IsValid();
        string GetName();
        void Refresh(bool volatileOnly);
        IEnumerable<IDevice> GetSubDevices();
        string GetLocalDeviceID();
        string GetUDID();
        IDevice GetParent();
    }
}
