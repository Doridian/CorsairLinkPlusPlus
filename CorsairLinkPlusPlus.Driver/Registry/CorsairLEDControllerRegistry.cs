using CorsairLinkPlusPlus.Driver.Controller.LED;
using CorsairLinkPlusPlus.Driver.Sensor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CorsairLinkPlusPlus.Driver.Registry
{
    public class CorsairLEDControllerRegistry : RegistryBase<CorsairLEDController>
    {
        private static Dictionary<byte, ConstructorInfo> ledControllers;

        static CorsairLEDControllerRegistry()
        {
            ledControllers = new Dictionary<byte, ConstructorInfo>();
            foreach(Type type in GetSubtypesInNamespace("CorsairLinkPlusPlus.Driver.Controller.LED"))
            {
                CorsairLEDController tempInstance = ConstructObjectForInspection(type);
                ledControllers.Add(tempInstance.GetLEDModernControllerID(), type.GetConstructor(new Type[0]));
            }
        }

        public static CorsairLEDController GetLEDController(CorsairLED led, byte modernTypeID)
        {
            if (!ledControllers.ContainsKey(modernTypeID))
                return null;
            CorsairLEDController ledController = (CorsairLEDController)ledControllers[modernTypeID].Invoke(new object[0]);
            ledController.AssignFrom(led);
            return ledController;
        }
    }
}
