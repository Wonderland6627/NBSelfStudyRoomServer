using NBSSR.Network;
using NBSSRServer.Logger;
using NBSSRServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Network
{
    internal class RequestInfo
    {
        public string ip;
        public string ua;
        public string headers;

        public RequestInfo(HttpListenerRequest request)
        {
            ip = request.RemoteEndPoint.Address.ToString();
            ua = request.UserAgent;
            headers = request.Headers.ToString();
        }

        public string GetIP()
        {
            return $"(ip: {ip})";
        }

        public string GetIPAndUA()
        {
            return $"(ip: {ip}, ua: {ua})";
        }

        public override string ToString()
        {
            return $"(ip: {ip}, ua: {ua}, headers: [\n{headers}])";
        }
    }

    internal partial class NetworkManager
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
        private NBSSRLogger logger = new("NetworkManager");

        public void Init(NBRouterBase router)
        {
            this.router = router;
        }

        public void ReadyToListen(string url)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(url);
            httpListener.Start();
            logger.LogInfo($"Server listen url: {url}");
        }

        public void Receiving()
        {
            if (httpListener == null)
            {
                return;
            }

            HttpListenerContext context = httpListener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            RequestInfo requestInfo = new(request);
            string json;
            using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8))
            {
                json = reader.ReadToEnd();
                logger.LogInfo($"Server listener get context: {json}, request info: {requestInfo}");
            }

            if (string.IsNullOrEmpty(json))
            {
                logger.LogError($"Server listener get empty input, request info: {requestInfo}");
                return;
            }

            try
            {
                object rawJsonObj = NetMsgSerializationHelper.Deserialize(json, out string errorMsg);
                if (errorMsg != "Success")
                {
                    logger.LogError($"Server deserialize json fail: {errorMsg}, json: {json}, request info: {requestInfo}");
                    return;
                }
                OnReceiveMessage(rawJsonObj, response, requestInfo);
            }
            catch (Exception ex)
            {
                logger.LogError($"Server deserialize json fail exception: {ex}, json: {json}, request info: {requestInfo}");
                return;
            }
        }

        private void OnReceiveMessage(object rawJsonObj, HttpListenerResponse response, RequestInfo requestInfo)
        {
            if (rawJsonObj is not NetMessageBase messageBase)
            {
                logger.LogWarning($"Server receive message is not type of NetMessageBase, request info: {requestInfo}");
                return;
            }

            OnReceiveMessage(messageBase, (rspObj) =>
            {
                string rspJson = NetMsgSerializationHelper.Serialize(rspObj);
                logger.LogInfo($"Server listener response context: {rspJson}, request ip: {requestInfo.GetIP()}");
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
