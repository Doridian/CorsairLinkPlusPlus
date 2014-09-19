using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Registry
{
    public class FanControllerRegistry : RegistryBase<FanController>
    {
        private static Dictionary<byte, ConstructorInfo> fanControllers;

        internal static void Initialize()
        {
            fanControllers = new Dictionary<byte, ConstructorInfo>();
            foreach(Type type in GetSubtypesInNamespace(Assembly.GetExecutingAssembly(), "CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan"))
            {
                FanController tempInstance = ConstructObjectForInspection(type);
                fanControllers.Add(tempInstance.GetFanModernControllerID(), type.GetConstructor(new Type[0]));
                ControllerRegistry.Get("CorsairLink." + type.Name, type);
            }
        }

        public static FanController Get(Fan fan, byte modernTypeID)
        {
            if (!fanControllers.ContainsKey(modernTypeID))
                return null;
            FanController fanController = (FanController)fanControllers[modernTypeID].Invoke(null);
            fanController.AssignFrom(fan);
            return fanController;
        }
    }
}
