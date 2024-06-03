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

        CreateStudentRequest = 1,
        CreateStudentResponse = 2,

        LoginRequest = 3,
        LoginResponse = 4,


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

    public partial class CreateStudentRequest : NetMessageBase
    {
        public CreateStudentRequest()
        {
            MessageType = NetMessageType.CreateStudentRequest;
        }

        public StudentInfo studentInfo { get; set; }
    }

    public partial class CreateStudentResponse : NetMessageResponseBase
    {
        public CreateStudentResponse()
        {
            MessageType = NetMessageType.CreateStudentResponse;
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