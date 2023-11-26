using NBSSRServer.MiniDatabase;
using NBSSRServer.Network;
using NBSSRServer.Services;
using NBSSRServer.Test;
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
            for (int i = 0; i < 10; i++)
            {
                MiniDataManager.Instance.userInfoDB.Add(MockData.GetRandomMockUserInfo());
            }

            NetworkManager.Instance.Init(new NBRouterBase());
        }
    }
}