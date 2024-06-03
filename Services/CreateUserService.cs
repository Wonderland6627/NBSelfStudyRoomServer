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
    public class CreateUserService : NBServiceBase<CreateStudentRequest, CreateStudentResponse>
    {
        public override CreateStudentResponse ProcessMessage(CreateStudentRequest request)
        {
            return CreateUser(request);
        }

        //根据传入的请求创建用户
        private CreateStudentResponse CreateUser(CreateStudentRequest request)
        {
            CreateStudentResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;
            StudentInfo studentInfo = request.studentInfo;
            if (studentInfo == null)
            {
                response.ErrorMsg = "empty student info.";
                return response;
            }

            if (string.IsNullOrEmpty(studentInfo.name) || string.IsNullOrEmpty(studentInfo.phone))
            {
                response.ErrorMsg = "empty student name or phone.";
                return response;
            }

            string account = studentInfo.phone;
            bool accountExist = CheckAccountExist(account);
            if (accountExist)
            {
                response.ErrorMsg = "account already exist.";
                return response;
            }

            AccountInfo accountInfo = new()
            {
                account = account,
                password = "",
                userID = MiniDataManager.Instance.accountInfoDB.datasList.Count + 1
            };
            studentInfo.userID = accountInfo.userID;

            MiniDataManager.Instance.accountInfoDB.Add(accountInfo);
            MiniDataManager.Instance.userInfoDB.Add(studentInfo, (data) => data.userID == accountInfo.userID);

            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"create student success: {accountInfo.Json()}");

            return response;
        }

        private bool CheckAccountExist(string account)
        {
            return MiniDataManager.Instance.accountInfoDB.datasList.Exists((accountInfo) => accountInfo.account == account);
        }
    }
}
