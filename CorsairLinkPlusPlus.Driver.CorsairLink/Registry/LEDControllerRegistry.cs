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

        static LEDControllerRegistry()
        {
            ledControllers = new Dictionary<byte, ConstructorInfo>();
            foreach(Type type in GetSubtypesInNamespace("CorsairLinkPlusPlus.Driver.Controller.LED"))
            {
                LEDController tempInstance = ConstructObjectForInspection(type);
                ledControllers.Add(tempInstance.GetLEDModernControllerID(), type.GetConstructor(new Type[0]));
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
