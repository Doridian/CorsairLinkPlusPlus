using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Registry
{
    public class LEDControllerRegistry : RegistryBase<LEDController>
    {
        private static Dictionary<byte, ConstructorInfo> ledControllers;

        internal static void Initialize()
        {
            ledControllers = new Dictionary<byte, ConstructorInfo>();
            foreach (Type type in GetSubtypesInNamespace(Assembly.GetExecutingAssembly(), "CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED"))
            {
                LEDController tempInstance = ConstructObjectForInspection(type);
                ledControllers.Add(tempInstance.GetLEDModernControllerID(), type.GetConstructor(new Type[0]));
                ControllerRegistry.Get("CorsairLink." + type.Name, type);
            }
        }

        public static LEDController Get(LED led, byte modernTypeID)
        {
            if (!ledControllers.ContainsKey(modernTypeID))
                return null;
            LEDController ledController = (LEDController)ledControllers[modernTypeID].Invoke(new object[0]);
            ledController.AssignFrom(led);
            return ledController;
        }
    }
}
