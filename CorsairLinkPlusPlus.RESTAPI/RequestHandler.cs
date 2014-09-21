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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CorsairLinkPlusPlus.RESTAPI
{
    public class RequestHandler
    {
        private ITcpChannel channel;
        private HttpRequestBase request;

        public RequestHandler(ITcpChannel channel, HttpRequestBase request)
        {
            this.channel = channel;
            this.request = request;
        }

        public void Start()
        {
            new Thread(new ThreadStart(_Start)).Start();
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

        private void RespondWithDevice(IDevice device)
        {
            device.Refresh(true);
            RespondWithJSON(device, true);
        }

        private void RespondWithJSON(object result, bool success = true, int statusCode = 200, Dictionary<string, string> headers = null)
        {
            string responseStr = JsonConvert.SerializeObject(new ResponseResult(result, success));
            byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(responseStr);
            MemoryStream output = new MemoryStream();
            output.Write(responseBytes, 0, responseBytes.Length);
            RespondWithRaw(output, statusCode, "application/json", headers);
        }

        private void RespondWithRaw(Stream body, int statusCode, string mimeType, Dictionary<string, string> headers = null)
        {
            IHttpResponse response = request.CreateResponse();

            response.ContentLength = (int)body.Length;
            response.StatusCode = statusCode;
            response.ContentType = mimeType;

            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Headers", "Authorization");
            response.AddHeader("Access-Control-Allow-Methods", "POST, HEAD, PUT, DELETE, GET, OPTIONS");
            if (headers != null)
                foreach (KeyValuePair<string, string> kvp in headers)
                    response.AddHeader(kvp.Key, kvp.Value);

            body.Position = 0;
            response.Body = body;

            channel.Send(response);

            request = null;
            channel = null;
        }

        private static bool AuthorizationValid(string auth)
        {
            if (!auth.StartsWith("Basic "))
                return false;

            auth = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6)));
            int authColon = auth.IndexOf(':');
            if (authColon <= 0)
                return false;

            string username = auth.Substring(0, authColon);
            if (!Program.validUsers.ContainsKey(username))
                return false;

            return Program.validUsers[username] == auth.Substring(authColon + 1);
        }

        private void _Start()
        {
            string absoluteUri = request.Uri.AbsolutePath;
            while (absoluteUri.IndexOf("//") >= 0)
                absoluteUri = absoluteUri.Replace("//", "/");

            if (absoluteUri.StartsWith("/api"))
            {
                if (!request.Headers.Contains("Authorization") || !AuthorizationValid(request.Headers["Authorization"]))
                {
                    RespondWithJSON("Please authenticate", false, 401, new Dictionary<string, string> { { "WWW-Authenticate", "Basic realm=\"CorsairLinkPlusPlus API\"" } });
                    return;
                }

                if (request.HttpMethod == "OPTIONS" || request.HttpMethod == "HEAD")
                {
                    RespondWithJSON("Yep yep, no need for these", true, 200);
                    return;
                }

                try
                {
                    IDevice device = RootDevice.FindDeviceByPath(absoluteUri.Substring(4));

                    if (device == null)
                    {
                        RespondWithJSON("Device not found", false, 404);
                        return;
                    }

                    RespondWithDevice(device);
                    return;
                }
                catch (Exception e)
                {
                    RespondWithJSON(e.Message, false, 500);
                    return;
                }
            }
            else
            {
                if (absoluteUri == "/" || absoluteUri == "")
                    absoluteUri = "index.html";
                else
                    absoluteUri = absoluteUri.Substring(1);

                FileInfo fileInfo = new FileInfo(Program.WEB_ROOT_ABSOLUTE + '/' + absoluteUri);

                if (!fileInfo.Exists)
                {
                    RespondWithJSON("Static content not found", false, 404);
                    return;
                }

                if (fileInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    RespondWithJSON("Static content is a directory", false, 403);
                    return;
                }

                DirectoryInfo directory = fileInfo.Directory;
                do
                {
                    if (Program.WEB_ROOT_ABSOLUTE.Equals(directory.FullName))
                        break;
                    directory = directory.Parent;
                } while (directory != null);

                if (directory == null)
                {
                    RespondWithJSON("Nope, sorry!", false, 403);
                    return;
                }

                string mineType = MIMEAssistant.GetMIMEType(fileInfo.Extension);

                RespondWithRaw(fileInfo.OpenRead(), 200, mineType);
            }
        }
    }
}
