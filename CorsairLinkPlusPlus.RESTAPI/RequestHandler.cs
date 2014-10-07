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

using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.RESTAPI.Methods;
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
        private volatile ITcpChannel channel;
        private HttpRequestBase request;
        private readonly object lockObject = new object();
        private UserData? currentUser = null;

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

        private bool RespondWithDevice(IDevice device)
        {
            device.Refresh(true);
            return RespondWithJSON(device, true);
        }

        private bool RespondWithJSON(object result, bool success = true, int statusCode = 200, Dictionary<string, string> headers = null)
        {
            string responseStr = JsonConvert.SerializeObject(new ResponseResult(result, success));
            byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(responseStr);
            MemoryStream output = new MemoryStream();
            output.Write(responseBytes, 0, responseBytes.Length);
            return RespondWithRaw(output, statusCode, "application/json", headers);
        }

        private bool RespondWithRaw(Stream body, int statusCode, string mimeType, Dictionary<string, string> headers = null)
        {
            ITcpChannel _channel;
            lock (lockObject)
            {
                if (channel == null)
                    return false;
                _channel = channel;
                channel = null;
            }

            try
            {
                IHttpResponse response = request.CreateResponse();
                request = null;

                response.ContentLength = (int)body.Length;
                response.StatusCode = statusCode;
                response.ContentType = mimeType;

                if (currentUser != null)
                {
                    response.AddHeader("X-User-Name", currentUser.Value.name);
                    response.AddHeader("X-User-Readonly", currentUser.Value.readOnly ? "Yes" : "No");
                }

                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("Access-Control-Allow-Headers", "Authorization");
                response.AddHeader("Access-Control-Allow-Methods", "POST, HEAD, PUT, DELETE, GET, OPTIONS");
                if (headers != null)
                    foreach (KeyValuePair<string, string> kvp in headers)
                        response.AddHeader(kvp.Key, kvp.Value);

                body.Position = 0;
                response.Body = body;

                _channel.Send(response);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private static UserData? GetCurrentUser(string auth)
        {
            if (!auth.StartsWith("Basic "))
                return null;

            auth = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6)));
            int authColon = auth.IndexOf(':');
            if (authColon <= 0)
                return null;

            string username = auth.Substring(0, authColon);
            if (!Program.validUsers.ContainsKey(username))
                return null;

            UserData ret = Program.validUsers[username];
            if (ret.password != auth.Substring(authColon + 1))
                return null;
            return ret;
        }

        class JSONMethodCall
        {
            public string Name;
            public Dictionary<string, object> Params;
        }

        private void _Start()
        {
            string absoluteUri = request.Uri.AbsolutePath;
            while (absoluteUri.IndexOf("//") >= 0)
                absoluteUri = absoluteUri.Replace("//", "/");

            if (absoluteUri.StartsWith("/api"))
            {
                if (!request.Headers.Contains("Authorization"))
                {
                    RespondWithJSON("Please authenticate", false, 401, new Dictionary<string, string> { { "WWW-Authenticate", "Basic realm=\"CorsairLinkPlusPlus API\"" } });
                    return;
                }

                currentUser = GetCurrentUser(request.Headers["Authorization"]);
                if (currentUser == null)
                {
                    RespondWithJSON("Invalid login", false, 401, new Dictionary<string, string> { { "WWW-Authenticate", "Basic realm=\"CorsairLinkPlusPlus API\"" } });
                    return;
                }

                if (request.HttpMethod == "OPTIONS" || request.HttpMethod == "HEAD")
                {
                    RespondWithJSON("Yep yep, no need for these");
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

                    if(request.HttpMethod == "POST")
                    {
                        if(currentUser.Value.readOnly)
                        {
                            RespondWithJSON("Your user is read-only", false, 403);
                            return;
                        }

                        JSONMethodCall methodCall;
                        using (StreamReader bodyReader = new StreamReader(request.Body))
                        {
                            methodCall = JsonConvert.DeserializeObject<JSONMethodCall>(bodyReader.ReadToEnd());
                        }
                        BaseMethod.Execute(methodCall.Name, device, methodCall.Params);
                        RespondWithJSON("OK");
                        return;
                    }

                    RespondWithDevice(device);
                    return;
                }
                catch (Exception e)
                {
                    try
                    {
                        RespondWithJSON(e.Message, false, 500);

                        Console.Error.WriteLine("------------------");
                        Console.Error.WriteLine("Error in HTTP");
                        Console.Error.WriteLine(e);
                        Console.Error.WriteLine("------------------");
                    }
                    catch (Exception se)
                    {
                        Console.Error.WriteLine("------------------");
                        Console.Error.WriteLine("Error in HTTP error handler");
                        Console.Error.WriteLine(se);
                        Console.Error.WriteLine("Caused by");
                        Console.Error.WriteLine(e);
                        Console.Error.WriteLine("------------------");
                    }
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
