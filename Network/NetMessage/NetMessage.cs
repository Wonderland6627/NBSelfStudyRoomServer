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

        CreateUserRequest = 0,
        CreateUserResponse = 1,

        LoginRequest = 2,
        LoginResponse = 3,
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
        public UserType userType;
        public int userID;
        public string userName;
        public UserGender userGender;
    }

    #endregion

    #region Net Class

    public class NetMessageBase
    {
        public NetMessageType MessageType { get; set; }
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

    public partial class TestResponse : NetMessageBase
    {
        public TestResponse()
        {
            MessageType = NetMessageType.TestResponse;
        }

        public int c { get; set; }
    }

    public partial class CreateUserRequest : NetMessageBase
    {
        public CreateUserRequest()
        {
            MessageType = NetMessageType.CreateUserRequest;
        }

        public UserType userType { get; set; }
        public string account { get; set; }
        public string password { get; set; }
    }

    public partial class CreateUserResponse : NetMessageBase
    {
        public CreateUserResponse()
        {
            MessageType = NetMessageType.CreateUserResponse;
        }

        public bool result { get; set; }
        public string errorMsg { get; set; }

        public UserType userType { get; set; }
    }

    public partial class LoginRequest : NetMessageBase
    {
        public LoginRequest()
        {
            MessageType = NetMessageType.LoginRequest;
        }

        public UserType userType { get; set; }
        public string account { get; set; }
        public string password { get; set; }
    }

    public partial class LoginResponse : NetMessageBase
    {
        public LoginResponse()
        {
            MessageType = NetMessageType.LoginResponse;
        }

        public bool result { get; set; }

        public string errorMsg { get; set; }

        public UserType userType { get; set; }

        public uint userID { get; set; }
    }

    #endregion
}