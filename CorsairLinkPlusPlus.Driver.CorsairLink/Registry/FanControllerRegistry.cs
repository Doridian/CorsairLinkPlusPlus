#region LICENSE
/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
 #endregion

using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Registry
{
    public class FanControllerRegistry : RegistryBase<IFanController>
    {
        private static Dictionary<byte, ConstructorInfo> fanControllers;

        internal static void Initialize()
        {
            fanControllers = new Dictionary<byte, ConstructorInfo>();
            foreach(Type type in GetSubtypesInNamespace(Assembly.GetExecutingAssembly(), "CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan"))
            {
                IFanController tempInstance = ConstructObjectForInspection(type);
                fanControllers.Add(tempInstance.GetFanModernControllerID(), type.GetConstructor(new Type[0]));
                ControllerRegistry.Get(tempInstance.Name, type);
            }
        }

        public static IFanController Get(Fan fan, byte modernTypeID)
        {
            if (!fanControllers.ContainsKey(modernTypeID))
                return null;
            IFanController fanController = (IFanController)fanControllers[modernTypeID].Invoke(null);
            fanController.AssignFrom(fan);
            return fanController;
        }
    }
}
