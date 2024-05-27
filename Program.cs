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

        private static string HttpUrl =
#if DEBUG
            "http://127.0.0.1:2333/";
#else
            "http://10.0.24.4:2333/";
#endif

        static void Main(string[] args)
        {
            try
            {
                if (args != null && args.Length > 0)
                {
                    HttpUrl = args[0];
                    logger.LogInfo(args);
                }
                Do();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                Console.ReadKey();
            }
        }

        static void Do()
        {
            bool test = false;
            if (test)
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
            }

            NetworkManager.Instance.Init(new NBRouterBase());
            NetworkManager.Instance.ReadyToListen(HttpUrl);
        }
    }
}