using System;
using System.IO;
using System.Net;

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
                using (StreamReader reader = new StreamReader(request.InputStream))
                {
                    json = reader.ReadToEnd();
                }

                count++;
                string responseString = $"request sent success {count}";
                Console.WriteLine($"Server listener get {responseString}");
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentType = "text/plain";
                response.ContentLength64 = buffer.Length;

                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }
    }
}