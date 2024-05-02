﻿using NBSSR.Network;
using NBSSRServer.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Services
{
    public interface INBService
    {
        NetMessageBase ProcessMessage(NetMessageBase requestMessage);
    }

    public abstract class NBServiceBase<Request, Response> : INBService 
        where Request : NetMessageBase 
        where Response : NetMessageBase
    {
        protected NBSSRLogger logger = new($"NBService<{RequestTypeName}, {ResponseTypeName}>");

        private static string RequestTypeName = typeof(Request).Name;
        private static string ResponseTypeName = typeof(Response).Name;

        public abstract Response ProcessMessage(Request request);

        public NetMessageBase ProcessMessage(NetMessageBase requestMessage)
        {
            Request req = requestMessage as Request;
            if (requestMessage is not Request request)
            {
                return null;
            }

            return ProcessMessage(request);
        }
    }
}
