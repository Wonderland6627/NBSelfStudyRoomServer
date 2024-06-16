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
    public class StudentInfoService : NBService
    {
        public static bool CheckAccountExist(string account)
        {
            return MiniDataManager.Instance.accountInfoDB.datasList.Exists((accountInfo) => accountInfo.account == account);
        }
    }

    public class CreateStudentInfoService : NBServiceBase<CreateStudentInfoRequest, CreateStudentInfoResponse>
    {
        public override CreateStudentInfoResponse ProcessMessage(CreateStudentInfoRequest request)
        {
            return CreateUser(request);
        }

        //根据传入的请求创建学生账号
        private CreateStudentInfoResponse CreateUser(CreateStudentInfoRequest request)
        {
            CreateStudentInfoResponse response = new();
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
            bool accountExist = StudentInfoService.CheckAccountExist(account);
            if (accountExist)
            {
                response.ErrorMsg = "account already exist.";
                return response;
            }

            int userID = MiniDataManager.Instance.accountInfoDB.datasList.Count;
            AccountInfo accountInfo = new()
            {
                account = account,
                password = "",
                userID = userID
            };
            studentInfo.userID = userID;

            MiniDataManager.Instance.accountInfoDB.Add(accountInfo);
            MiniDataManager.Instance.userInfoDB.Add(studentInfo);

            response.studentInfo = studentInfo;
            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"create student success: {studentInfo.Json()}");

            return response;
        }
    }
}
