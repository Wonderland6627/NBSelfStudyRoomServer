using NBSSR.Network;
using NBSSRServer.Logger;
using NBSSRServer.MiniDatabase;
using NBSSRServer.Network;
using NBSSRServer.Services;
using NBSSRServer.Tests;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace NBSSRServer
{
    class Program
    {
        private static NBSSRLogger logger = new("Program");

        private static readonly string HttpUrl = "http://localhost:5000/";

        static void Main(string[] args)
        {
            try
            {
                Do();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }
        }

        static void Do()
        {
            for (int i = 0; i < 10; i++)
            {
                UserInfo user = MockData.GetRandomMockUserInfo();
                MiniDataManager.Instance.userInfoDB.Add(user);
                NBSSRLogWriter.Log("T", user);
            }

            MiniDatabaseTests test1 = new MiniDatabaseTests();
            for (int i = 0; i < 10; i++)
            {
                test1.SimulateRandomOperations();
                Thread.Sleep(1000);
            }

            NetworkManager.Instance.Init(new NBRouterBase());
            NetworkManager.Instance.ReadyToListen(HttpUrl);
        }
    }
}