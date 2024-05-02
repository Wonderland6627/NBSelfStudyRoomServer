using NBSSR.Network;
using NBSSRServer.Extensions;
using NBSSRServer.Logger;
using NBSSRServer.MiniDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Services
{
    public class CreateUserService : NBServiceBase<CreateUserRequest, CreateUserResponse>
    {
        public override CreateUserResponse ProcessMessage(CreateUserRequest request)
        {
            return CreateUser(request);
        }

        //根据传入的请求创建用户
        private CreateUserResponse CreateUser(CreateUserRequest request)
        {
            CreateUserResponse response = new();
            response.ActionCode = NetMessageActionCode.Error;
            UserInfo userInfo = request.userInfo;
            if (userInfo == null)
            {
                response.result = false;
                response.ErrorMsg = "empty user info.";
                return response;
            }

            if (string.IsNullOrEmpty(userInfo.name) || string.IsNullOrEmpty(userInfo.phone))
            {
                response.result = false;
                response.ErrorMsg = "empty user name or phone.";
                return response;
            }

            string account = GenerateAccount(userInfo.phone);
            bool accountExist = CheckAccountExist(account);
            if (accountExist)
            {
                response.result = false;
                response.ErrorMsg = "account already exist.";
                return response;
            }

            string password = GeneratePassword(account);
            AccountInfo accountInfo = new()
            {
                account = account,
                password = password,
                userID = MiniDataManager.Instance.accountInfoDB.datasList.Count + 1
            };
            userInfo.userID = accountInfo.userID;

            MiniDataManager.Instance.accountInfoDB.Add(accountInfo);
            MiniDataManager.Instance.userInfoDB.Add(userInfo, (data) => data.userID == accountInfo.userID);

            response.result = true;
            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"create user success: {accountInfo.Json()}");

            return response;
        }

        private string GenerateAccount(string phone)
        {
            return phone;
        }

        private string GeneratePassword(string account)
        {
            return account;
        }

        private bool CheckAccountExist(string account)
        {
            return MiniDataManager.Instance.accountInfoDB.datasList.Exists((accountInfo) => accountInfo.account == account);
        }
    }
}
