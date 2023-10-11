using NBSSR.Network;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace NBSSRServer
{
    class Program
    {
        static void Main(string[] args)
        {
            AddListener();
        }

        static void AddListener()
        {
            string url = "http://localhost:5000/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine($"Server listener start {url}");

            int count = 0;

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string json;
                using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                }

                Console.WriteLine($"Server listener get context: {json}");

                TestResponse rspObj = new TestResponse();
                rspObj.state = true;
                string rspJson = NetMsgSerializationHelper.Serialize(rspObj);
                Console.WriteLine($"Server listener response context: {rspJson}");
                byte[] buffer = Encoding.UTF8.GetBytes(rspJson);

                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;

                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }
    }
}