using NBSSR.Network;
using NBSSRServer.Network;
using NBSSRServer.Services;
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
            NetworkManager.Instance.Init(new NBRouterBase());
        }
    }
}