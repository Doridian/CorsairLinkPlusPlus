using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Sensor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CorsairLinkPlusPlus.Driver.Registry
{
    public class CorsairFanControllerRegistry : RegistryBase<CorsairFanController>
    {
        private static Dictionary<byte, ConstructorInfo> fanControllers;

        static CorsairFanControllerRegistry()
        {
            fanControllers = new Dictionary<byte, ConstructorInfo>();
            foreach(Type type in GetSubtypesInNamespace("CorsairLinkPlusPlus.Driver.Controller.Fan"))
            {
                CorsairFanController tempInstance = ConstructObjectForInspection(type);
                fanControllers.Add(tempInstance.GetFanModernControllerID(), type.GetConstructor(new Type[0]));
            }
        }

        public static CorsairFanController GetFanController(CorsairFan fan, byte modernTypeID)
        {
            if (!fanControllers.ContainsKey(modernTypeID))
                return null;
            CorsairFanController fanController = (CorsairFanController)fanControllers[modernTypeID].Invoke(new object[0]);
            fanController.AssignFrom(fan);
            return fanController;
        }
    }
}
