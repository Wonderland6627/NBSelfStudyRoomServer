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

        public static StudentInfo GetStudentInfo(int id)
        {
            return MiniDataManager.Instance.studentInfoDB.Get((item) => item.floorID == id);
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
                response.ErrorMsg = "empty param student info.";
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
            MiniDataManager.Instance.studentInfoDB.Add(studentInfo);

            response.studentInfo = studentInfo;
            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"create student success: {studentInfo.Json()}");

            return response;
        }
    }

    public class GetStudentInfoService : NBServiceBase<GetStudentInfoRequest, GetStudentInfoResponse>
    {
        public override GetStudentInfoResponse ProcessMessage(GetStudentInfoRequest request)
        {
            return GetStudentInfo(request);
        }

        private GetStudentInfoResponse GetStudentInfo(GetStudentInfoRequest request)
        {
            GetStudentInfoResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;
            int id = request.studentID;

            StudentInfo dbStudentInfo = StudentInfoService.GetStudentInfo(id);
            if (dbStudentInfo == null)
            {
                response.ErrorMsg = $"student info not exist, id: {id}";
                return response;
            }

            response.studentInfo = dbStudentInfo;
            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"get student info success: {dbStudentInfo.Json()}");

            return response;
        }
    }

    public class UpdateStudentInfoService : NBServiceBase<UpdateStudentInfoRequest, UpdateStudentInfoResponse>
    {
        public override UpdateStudentInfoResponse ProcessMessage(UpdateStudentInfoRequest request)
        {
            return UpdateStudentInfo(request);
        }

        private UpdateStudentInfoResponse UpdateStudentInfo(UpdateStudentInfoRequest request)
        {
            UpdateStudentInfoResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;
            StudentInfo studentInfo = request.studentInfo;
            if (studentInfo == null)
            {
                response.ErrorMsg = "empty param student info.";
                return response;
            }

            int id = studentInfo.userID;
            StudentInfo dbStudentInfo = StudentInfoService.GetStudentInfo(id);
            if (dbStudentInfo == null)
            {
                response.ErrorMsg = $"student info not exist, id: {id}";
                return response;
            }

            MiniDataManager.Instance.studentInfoDB.Update((item) => item.userID == id, studentInfo);
            string phone = studentInfo.phone;
            string dbPhone = dbStudentInfo.phone;
            if (phone != dbPhone) //修改手机号了 对应登陆accountinfo也做修改
            {
                AccountInfo accountInfo = MiniDataManager.Instance.accountInfoDB.Get((item) => item.userID == id);
                accountInfo.account = phone;
                MiniDataManager.Instance.accountInfoDB.Update((item) => item.userID == id, accountInfo);
            }

            response.studentInfo = studentInfo;
            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"update student info success: {studentInfo.Json()}");

            return response;
        }
    }

    public class DeleteStudentInfoService : NBServiceBase<DeleteStudentInfoRequest, DeleteStudentInfoResponse>
    {
        public override DeleteStudentInfoResponse ProcessMessage(DeleteStudentInfoRequest request)
        {
            return DeleteStudentInfo(request);
        }

        private DeleteStudentInfoResponse DeleteStudentInfo(DeleteStudentInfoRequest request)
        {
            DeleteStudentInfoResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;
            int id = request.studentID;

            StudentInfo dbStudentInfo = StudentInfoService.GetStudentInfo(id);
            if (dbStudentInfo == null)
            {
                response.ErrorMsg = $"student info not exist, id: {id}";
                return response;
            }

            if (!MiniDataManager.Instance.studentInfoDB.Remove((item) => item.userID == id))
            {
                response.ErrorMsg = $"delete student info failed, id: {id}";
                return response;
            }

            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"delete student info success: {id}");

            return response;
        }
    }
}
