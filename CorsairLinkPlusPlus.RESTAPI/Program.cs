using CorsairLinkPlusPlus.Common;
using Griffin.Net.Channels;
using Griffin.Net.Protocols.Http;
using Griffin.Net.Protocols.Serializers;
using Newtonsoft.Json;
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
            HttpListener httpServer = new HttpListener();
            httpServer.ClientConnected += httpServer_ClientConnected;
            httpServer.MessageReceived = OnMessage;
            httpServer.BodyDecoder = new CompositeIMessageSerializer();
            httpServer.Start(IPAddress.Loopback, 38012);

            Console.ReadLine();
        }

        private static void httpServer_ClientConnected(object sender, Griffin.Net.Protocols.ClientConnectedEventArgs e)
        {
            
        }

        private static void RespondWithDevice(ITcpChannel channel, HttpRequestBase request, IDevice device)
        {
            RespondWith(channel, request, device, true);
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

        private static void RespondWith(ITcpChannel channel, HttpRequestBase request, object result, bool success = true)
        {
            IHttpResponse response = request.CreateResponse();

            string responseStr = JsonConvert.SerializeObject(new ResponseResult(result, success));
            byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(responseStr);
            MemoryStream output = new MemoryStream();
            output.Write(responseBytes, 0, responseBytes.Length);
            output.Position = 0;
            response.Body = output;
            response.ContentLength = responseBytes.Length;

            channel.Send(response);
        }

        private static void OnMessage(ITcpChannel channel, object message)
        {
            HttpRequestBase request = (HttpRequestBase)message;

            IDevice device = RootDevice.FindDeviceByPath(request.Uri.AbsolutePath);

            if(device == null)
            {
                RespondWith(channel, request, "Not found", false);
                return;
            }

            RespondWithDevice(channel, request, device);
        }
    }
}
