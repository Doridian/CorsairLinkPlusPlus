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

using Griffin.Net;
using Griffin.Net.Channels;
using Griffin.Net.Protocols.Http;
using Griffin.Net.Protocols.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using HttpListener = Griffin.Net.Protocols.Http.HttpListener;

namespace CorsairLinkPlusPlus.RESTAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            });

            AddUser(new UserData("root", "root", false));

            HttpListener httpServer = new HttpListener(new ChannelTcpListenerConfiguration(
                () => new HttpMessageDecoder(),
                () => new HttpMessageEncoder()
            ));
            httpServer.ClientConnected += httpServer_ClientConnected;
            httpServer.MessageReceived = OnMessage;
            httpServer.Start(IPAddress.Any, 38012);

            //CLI MENU
            while (true)
            {
                Console.Out.WriteLine("========================================");
                Console.Out.WriteLine("Menu:");
                Console.Out.WriteLine("1) List users");
                Console.Out.WriteLine("2) Add/Edit user");
                Console.Out.WriteLine("3) Delete user");
                Console.Out.WriteLine("9) Exit program");
                Console.Out.Write("Choice: ");
                string picked = Console.In.ReadLine();
                Console.Out.WriteLine("----------------------------------------");
                switch(picked)
                {
                    case "1":
                        foreach(UserData userData in validUsers.Values)
                            Console.Out.WriteLine("Name: " + userData.name + ", ReadOnly: " + (userData.readOnly ? "Yes" : "No"));
                        break;
                    case "2":
                        UserData user = ConsoleReadUser();
                        validUsers[user.name] = user;
                        break;
                    case "3":
                        Console.Out.Write("Username: ");
                        string username = Console.In.ReadLine();
                        validUsers.Remove(username);
                        break;
                    case "9":
                        httpServer.Stop();
                        return;
                }
            }
        }

        private static UserData ConsoleReadUser()
        {
            Console.Out.Write("Username: ");
            string username = Console.In.ReadLine();
            Console.Out.Write("Password: ");
            string password = Console.In.ReadLine();
            Console.Out.Write("Read only (yes/no): ");
            bool readOnly = Console.In.ReadLine().StartsWith("y");
            return new UserData(username, password, readOnly);
        }

        internal static string WEB_ROOT_ABSOLUTE = new DirectoryInfo("./web").FullName;

        internal static Dictionary<string, UserData> validUsers = new Dictionary<string, UserData>();

        private static void AddUser(UserData user)
        {
            validUsers[user.name] = user;
        }

        private static void httpServer_ClientConnected(object sender, Griffin.Net.Protocols.ClientConnectedEventArgs e)
        {
            
        }

        private static void OnMessage(ITcpChannel channel, object message)
        {
            new RequestHandler(channel, (HttpRequestBase)message).Start();
        }
    }
}