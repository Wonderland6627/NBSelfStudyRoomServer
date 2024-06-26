﻿using NBSSR.Network;
using NBSSRServer.Extensions;
using NBSSRServer.MiniDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Services
{
    public class LoginService : NBServiceBase<LoginRequest, LoginResponse>
    {
        public override LoginResponse ProcessMessage(LoginRequest request)
        {
            return Login(request);
        }

        private LoginResponse Login(LoginRequest request)
        {
            LoginResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;

            AccountInfo accountInfo = request.accountInfo;
            var dbUser = MiniDataManager.Instance.accountInfoDB.Get((data) => data.account.Equals(accountInfo.account));
            if (dbUser == null)
            {
                response.ErrorMsg = "can not find user, account does not exist.";
                return response;
            }

            if (!accountInfo.password.Equals(dbUser.password))
            {
                response.ErrorMsg = $"can not match password, request: {request.Json()}, db: {dbUser.Json()}";
                return response;
            }

            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"login success: {request.Json()}");

            return response;
        }
    }
}
