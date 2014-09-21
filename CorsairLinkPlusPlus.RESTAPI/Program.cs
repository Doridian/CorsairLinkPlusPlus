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
using CorsairLinkPlusPlus.Common;
using Griffin.Net.Channels;
using Griffin.Net.Protocols.Http;
using Griffin.Net.Protocols.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Net;
using System.Threading;
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

        private static void httpServer_ClientConnected(object sender, Griffin.Net.Protocols.ClientConnectedEventArgs e)
        {
            
        }

        private class ResponseResult
        {
            public bool success;
            public object result;

            internal ResponseResult(object result, bool success)
            {
                this.success = success;
                this.result = result;
            }
        }

        private static void RespondWithDevice(ITcpChannel channel, HttpRequestBase request, IDevice device)
        {
            device.Refresh(true);
            RespondWithJSON(channel, request, device, true);
        }

        private static void RespondWithJSON(ITcpChannel channel, HttpRequestBase request, object result, bool success = true, int statusCode = 200)
        {
            string responseStr = JsonConvert.SerializeObject(new ResponseResult(result, success));
            byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(responseStr);
            MemoryStream output = new MemoryStream();
            output.Write(responseBytes, 0, responseBytes.Length);
            RespondWithRaw(channel, request, output, statusCode, "application/json");
        }

        private static void RespondWithRaw(ITcpChannel channel, HttpRequestBase request, Stream body, int statusCode, string mimeType)
        {
            IHttpResponse response = request.CreateResponse();

            response.ContentLength = (int)body.Length;
            response.StatusCode = statusCode;
            response.ContentType = mimeType;
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Methods", "POST, HEAD, PUT, DELETE, GET, OPTIONS");
            body.Position = 0;
            response.Body = body;

            channel.Send(response);
        }

        private static string WEB_ROOT_ABSOLUTE = new DirectoryInfo("./web").FullName;

        private static void OnMessage(ITcpChannel channel, object message)
        {
            HttpRequestBase request = (HttpRequestBase)message;

            if(request.HttpMethod == "OPTIONS" || request.HttpMethod == "HEAD")
            {
                RespondWithJSON(channel, request, "Yep yep, no need for these", true, 200);
                return;
            }

            string absoluteUri = request.Uri.AbsolutePath;
            while (absoluteUri.IndexOf("//") >= 0)
                absoluteUri = absoluteUri.Replace("//", "/");

            if(absoluteUri == "/web" || absoluteUri == "/web/")
                absoluteUri = "/web/index.htm";

            if(absoluteUri.StartsWith("/web/"))
            {
                string path = absoluteUri.Substring(5);

                FileInfo fileInfo = new FileInfo(WEB_ROOT_ABSOLUTE + '/' + path);

                if(!fileInfo.Exists)
                {
                    RespondWithJSON(channel, request, "Static content not found", false, 404);
                    return;
                }

                if(fileInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    RespondWithJSON(channel, request, "Static content is a directory", false, 403);
                    return;
                }

                DirectoryInfo directory = fileInfo.Directory;
                do {
                    if (WEB_ROOT_ABSOLUTE.Equals(directory.FullName))
                        break;
                    directory = directory.Parent;
                } while (directory != null);

                if(directory == null)
                {
                    RespondWithJSON(channel, request, "Nope, sorry!", false, 403);
                    return;
                }

                string mineType = MIMEAssistant.GetMIMEType(fileInfo.Extension);

                RespondWithRaw(channel, request, fileInfo.OpenRead(), 200, mineType);
            }

            try
            {
                IDevice device = RootDevice.FindDeviceByPath(absoluteUri);

                if (device == null)
                {
                    RespondWithJSON(channel, request, "Device not found", false, 404);
                    return;
                }

                RespondWithDevice(channel, request, device);
            }
            catch (Exception e)
            {
                RespondWithJSON(channel, request, e.Message, false, 500);
            }
        }
    }
}
