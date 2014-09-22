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
using CorsairLinkPlusPlus.Common.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CorsairLinkPlusPlus.Common
{
    public class RootDevice : BaseDevice
    {
        private static readonly object instanceLock = new object();
        private volatile static RootDevice instance = null;

        private List<IDevice> rootDevices = new List<IDevice>();

        private volatile bool initialized = false;

        private RootDevice()
            : base(null)
        {

        }

        public static RootDevice GetInstance()
        {
            lock (instanceLock)
            {
                if (instance == null)
                    instance = new RootDevice();
            }

            return instance;
        }

        public static IDevice FindDeviceByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return GetInstance();

            if (path[0] == '/')
            {
                if (path.Length == 1)
                    return GetInstance();
                path = path.Substring(1);
            }

            return GetInstance().FindBySubPath(path);
        }

        public void Initialize()
        {
            lock (subDeviceLock)
            {
                if (initialized)
                    return;
                initialized = true;
            }

            string path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOfAny(new char[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar }));
            foreach (string file in Directory.EnumerateFiles(path, "CorsairLinkPlusPlus.Driver.*.dll"))
            {
                Assembly driver = Assembly.LoadFile(file);
                Type iRootDevice = typeof(IRootDevice);
                foreach (Type t in driver.GetExportedTypes())
                {
                    if (!t.IsAbstract && !t.IsInterface && iRootDevice.IsAssignableFrom(t))
                    {
                        try
                        {
                            IRootDevice rootDevice = (IRootDevice)t.GetConstructor(new Type[0]).Invoke(null);
                            rootDevice.Initialize();
                            rootDevices.Add(rootDevice);
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine("Could not load driver " + driver.FullName + ": " + e.Message);
                        }
                    }
                }
            }
        }

        public override DeviceType Type
        {
            get
            {
                return DeviceType.Root;
            }
        }

        public override string Name
        {
            get
            {
                return "Root Device";
            }
        }

        public override string GetLocalDeviceID()
        {
            return "";
        }

        public override IEnumerable<IDevice> GetSubDevices()
        {
            Initialize();
            return rootDevices;
        }
    }
}
