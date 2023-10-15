using NBSSR.Network;
using NBSSRServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Network
{
    internal class NetworkManager
    {
        private static NetworkManager instance;

        public static NetworkManager Instance
        { 
            get 
            {
                if (instance == null)
                {
                    instance = new NetworkManager();
                }
                return instance;
            }
        }

        public NetworkManager() { }

        private HttpListener httpListener;
        private NBRouterBase router;

        public void Init(NBRouterBase router)
        {
            this.router = router;
            AddListener("http://localhost:5000/");
        }

        private void AddListener(string url)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(url);
            httpListener.Start();
            Console.WriteLine($"Server listen url: {url}");

            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string json;
                using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                    Console.WriteLine($"Server listener get context: {json}");
                }

                try
                {
                    object rawJsonObj = NetMsgSerializationHelper.Deserialize(json, out string errorMsg);
                    if (errorMsg != "Success")
                    {
                        Console.WriteLine($"Server deserialize json fail: {errorMsg}, json: {json}");
                        continue;
                    }
                    OnReceiveMessage(rawJsonObj, response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Server deserialize json fail exception: {ex}, json: {json}");
                    continue;
                }
            }
        }

        private void OnReceiveMessage(object rawJsonObj, HttpListenerResponse response)
        {
            if (rawJsonObj is not NetMessageBase messageBase)
            {
                Console.WriteLine($"Server receive message is not type of NetMessageBase");
                return;
            }

            OnReceiveMessage(messageBase, (rspObj) =>
            {
                string rspJson = NetMsgSerializationHelper.Serialize(rspObj);
                Console.WriteLine($"Server listener response context: {rspJson}");
                byte[] buffer = Encoding.UTF8.GetBytes(rspJson);

                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;

                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            });
        }

        private void OnReceiveMessage(NetMessageBase netMessageBase, Action<NetMessageBase> callBack = null)
        {
            if (router == null)
            {
                return;
            }

            var message = router.RouteMessage(netMessageBase);
            callBack?.Invoke(message);
        }
    }
}
