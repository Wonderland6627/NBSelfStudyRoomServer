using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NBSSR.Network
{
    #region Enum

    [JsonConverter(typeof(StringEnumConverter))]
    public enum NetMessageType
    {
        TestRequest = -2,
        TestResponse = -1,

        ErrorMessage = 0,

        LoginRequest = 1,
        LoginResponse = 2,

        CreateStudentInfoRequest = 3,
        CreateStudentInfoResponse = 4,

        GetStudentInfoRequest = 5,
        GetStudentInfoResponse = 6,

        UpdateStudentInfoRequest = 7,
        UpdateStudentInfoResponse = 8,

        DeleteStudentInfoRequest = 9,
        DeleteStudentInfoResponse = 10,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum NetMessageActionCode
    {
        None = 0,
        Success = 1,
        Failed = 2,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserType
    {
        Unknown,
        Admin,
        Student,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserGender
    {
        Unknown,
        Female,
        Male,
    }

    #endregion

    #region Class

    public partial class UserInfo
    {
        public int userID;
        public UserType userType;
    }

    public partial class StudentInfo : UserInfo
    {
        public string name;
        public UserGender gender;
        public DateTime birthday;
        public string phone;

        public int enrollmentYear; //入学年份
        public int gradeDifference; //跳级/留级差
        public string school;
        public string @class;

        public string qq;
        public string wechat;

        public bool isGraduated; //是否已经毕业
    }

    #endregion

    #region Net Class

    public class NetMessageBase
    {
        public NetMessageType MessageType { get; set; }
    }

    public class NetMessageResponseBase : NetMessageBase
    {
        public NetMessageActionCode ActionCode { get; set; } = NetMessageActionCode.None;
        public string ErrorMsg { get; set; }
    }

    public partial class TestRequest : NetMessageBase
    {
        public TestRequest()
        {
            MessageType = NetMessageType.TestRequest;
        }

        public int a { get; set; }
        public int b { get; set; }
    }

    public partial class TestResponse : NetMessageResponseBase
    {
        public TestResponse()
        {
            MessageType = NetMessageType.TestResponse;
        }

        public int c { get; set; }
    }

    public partial class CreateStudentInfoRequest : NetMessageBase
    {
        public CreateStudentInfoRequest()
        {
            MessageType = NetMessageType.CreateStudentInfoRequest;
        }

        public StudentInfo studentInfo { get; set; }
    }

    public partial class CreateStudentInfoResponse : NetMessageResponseBase
    {
        public CreateStudentInfoResponse()
        {
            MessageType = NetMessageType.CreateStudentInfoResponse;
        }
    }

    public partial class LoginRequest : NetMessageBase
    {
        public LoginRequest()
        {
            MessageType = NetMessageType.LoginRequest;
        }

        public string account { get; set; }
        public string password { get; set; }
    }

    public partial class LoginResponse : NetMessageResponseBase
    {
        public LoginResponse()
        {
            MessageType = NetMessageType.LoginResponse;
        }

        public UserInfo userInfo { get; set; }
    }

    #endregion
}