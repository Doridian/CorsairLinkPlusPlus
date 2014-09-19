using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common
{
    public interface IDevice
    {
        bool Present { get; }
        void Disable();
        bool Valid { get; }
        string Name { get; }
        void Refresh(bool volatileOnly);
        IEnumerable<IDevice> GetSubDevices();
        IEnumerable<string> ChildrenPaths { get; }
        string GetLocalDeviceID();
        string AbsolutePath { get; }
        IDevice GetParent();
        string ParentPath { get; }
        IDevice FindBySubPath(string subPath);
    }
}
