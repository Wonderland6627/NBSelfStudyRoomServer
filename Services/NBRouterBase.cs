using NBSSR.Network;
using NBSSRServer.Logger;
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
        private NBSSRLogger logger = new("NBRouterBase");

        public NBRouterBase()
        {
            serviceMap = new Dictionary<NetMessageType, INBService>();
            RegisterService(NetMessageType.TestRequest, new TestService());
            RegisterService(NetMessageType.LoginRequest, new LoginService());
            RegisterService(NetMessageType.CreateStudentInfoRequest, new CreateStudentInfoService());
            RegisterService(NetMessageType.GetStudentInfoRequest, new GetStudentInfoService());
            RegisterService(NetMessageType.UpdateStudentInfoRequest, new UpdateStudentInfoService());
            RegisterService(NetMessageType.DeleteStudentInfoRequest, new DeleteStudentInfoService());
            RegisterService(NetMessageType.CreateSeatRequest, new CreateSeatService());
            RegisterService(NetMessageType.UpdateSeatRequest, new UpdateSeatService());
            RegisterService(NetMessageType.DeleteSeatRequest, new DeleteSeatService());
        }

        public void RegisterService(NetMessageType type, INBService service)
        {
            serviceMap[type] = service;
        }

        public NetMessageBase RouteMessage(NetMessageBase message)
        {
            if (!serviceMap.ContainsKey(message.MessageType))
            {
                logger.LogWarning($"can not match router service, check serviceMap");
                return null;
            }

            INBService service = serviceMap[message.MessageType];
            logger.LogInfo($"Router message type: {message.GetType()}");
            return service.ProcessMessage(message);
        }
    }
}
