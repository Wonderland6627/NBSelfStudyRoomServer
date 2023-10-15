using NBSSR.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Services
{
    public class NBRouterBase
    {
        private Dictionary<NetMessageType, INBService> serviceMap;

        public NBRouterBase()
        {
            serviceMap = new Dictionary<NetMessageType, INBService>();
            RegisterService(NetMessageType.TestRequest, new TestService());
        }

        public void RegisterService(NetMessageType type, INBService service)
        {
            serviceMap[type] = service;
        }

        public NetMessageBase RouteMessage(NetMessageBase message)
        {
            if (!serviceMap.ContainsKey(message.MessageType))
            {
                return null;
            }

            INBService service = serviceMap[message.MessageType];
            Console.WriteLine($"Router message type: {message.GetType()}");
            return service.ProcessMessage(message);
        }
    }
}
