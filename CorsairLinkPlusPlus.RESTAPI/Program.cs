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
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return settings;
            });

            HttpListener httpServer = new HttpListener();
            httpServer.ClientConnected += httpServer_ClientConnected;
            httpServer.MessageReceived = OnMessage;
            httpServer.BodyDecoder = new CompositeIMessageSerializer();
            httpServer.Start(IPAddress.Any, 38012);

            Console.ReadLine();
        }

        internal static string WEB_ROOT_ABSOLUTE = new DirectoryInfo("./web").FullName;

        internal static Dictionary<string, string> validUsers = new Dictionary<string, string> {
            { "root", "root" }
        };

        private static void httpServer_ClientConnected(object sender, Griffin.Net.Protocols.ClientConnectedEventArgs e)
        {
            
        }

        private static void OnMessage(ITcpChannel channel, object message)
        {
            new RequestHandler(channel, (HttpRequestBase)message).Start();
        }
    }
}
