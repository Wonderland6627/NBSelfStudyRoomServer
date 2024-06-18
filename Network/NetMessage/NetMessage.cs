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

        //基础接口
        CheckUpdateRequest = 1,
        CheckUpdateResponse = 2,

        LoginRequest = 3,
        LoginResponse = 4,

        //学生信息
        CreateStudentInfoRequest = 101,
        CreateStudentInfoResponse = 102,

        GetStudentInfoRequest = 103,
        GetStudentInfoResponse = 104,

        UpdateStudentInfoRequest = 105,
        UpdateStudentInfoResponse = 106,

        DeleteStudentInfoRequest = 107,
        DeleteStudentInfoResponse = 108,

        //座位信息
        CreateSeatRequest = 201,
        CreateSeatResponse = 202,

        UpdateSeatRequest = 203,
        UpdateSeatResponse = 204,

        DeleteSeatRequest = 205,
        DeleteSeatResponse = 206,

        //套餐信息
        CreatePackageRequest = 301,
        CreatePackageResponse = 302,

        UpdatePackageRequest = 303,
        UpdatePackageResponse = 304,

        DeletePackageRequest = 305,
        DeletePackageResponse = 306,
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

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PackageType
    {
        Temp,
        Week,
        Month,
        Season,
        Year,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SeatType
    {
        A,
        B,
        C,
        D,
    }

    #endregion

    #region Class

    public partial class AccountInfo
    {
        public string account;
        public string password;

        public int userID;
    }

    public partial class UserInfo
    {
        public int userID;
        public UserType userType;
    }

    public partial class AdminInfo: UserInfo
    {

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

        public int storeID;
        public int floorID;
        public int seatID;
        public PackageType packageType; //套餐类型
        public DateTime expirationDate; //到期时间
    }

    public partial class Store
    {
        public int storeID;
        public string storeName;

        public List<Floor> floors;
    }

    public partial class Floor
    {
        public int floorID, storeID;
        public string floorName;
        public Size size; //楼层尺寸
    }

    public partial class Seat
    {
        public int seatID, floorID, storeID;
        public string seatName;
        public Position position; //座位在楼层的位置（坐标）

        public SeatType seatType; //座位类型
        public int studentID;
        public bool isOccupied;
    }

    public partial class Size
    {
        public int width, length;

        public Size(int width, int length)
        {
            this.width = width;
            this.length = length;
        }
    }

    public partial class Position
    {
        public int x, y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    //套餐信息 不同座位 不同时长
    public class PackageInfo
    {
        public SeatType seatType;
        public PackageType packageType;
        public float price; //价格
        public float discount; //折扣
        public int giftDayCount; //赠送天数
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

    public partial class LoginRequest : NetMessageBase
    {
        public LoginRequest()
        {
            MessageType = NetMessageType.LoginRequest;
        }

        public AccountInfo accountInfo { get; set; }
    }

    public partial class LoginResponse : NetMessageResponseBase
    {
        public LoginResponse()
        {
            MessageType = NetMessageType.LoginResponse;
        }

        public UserInfo userInfo { get; set; }
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

        public StudentInfo studentInfo { get; set; }
    }

    public partial class GetStudentInfoRequest: NetMessageBase
    {
        public GetStudentInfoRequest()
        {
            MessageType = NetMessageType.GetStudentInfoRequest;
        }

        public int studentID { get; set; }
    }

    public partial class GetStudentInfoResponse: NetMessageResponseBase
    {
        public GetStudentInfoResponse()
        {
            MessageType = NetMessageType.GetStudentInfoResponse;
        }

        public StudentInfo studentInfo { get; set; }
    }

    public partial class UpdateStudentInfoRequest: NetMessageBase 
    {
        public UpdateStudentInfoRequest()
        {
            MessageType = NetMessageType.UpdateStudentInfoRequest;
        }

        public StudentInfo studentInfo { get; set; }
    }

    public partial class UpdateStudentInfoResponse: NetMessageResponseBase 
    {
        public UpdateStudentInfoResponse()
        {
            MessageType = NetMessageType.UpdateStudentInfoResponse;
        }

        public StudentInfo studentInfo { get; set; }
    }

    public partial class DeleteStudentInfoRequest: NetMessageBase 
    {
        public DeleteStudentInfoRequest()
        {
            MessageType = NetMessageType.DeleteStudentInfoRequest;
        }

        public int studentID { get; set; }
    }

    public partial class DeleteStudentInfoResponse: NetMessageResponseBase 
    {
        public DeleteStudentInfoResponse()
        {
            MessageType = NetMessageType.DeleteStudentInfoResponse;
        }
    }

    public partial class CreateSeatRequest: NetMessageBase
    {
        public CreateSeatRequest()
        {
            MessageType = NetMessageType.CreateSeatRequest;
        }

        public Seat seat { get; set; }
    }

    public partial class CreateSeatResponse: NetMessageResponseBase
    {
        public CreateSeatResponse()
        {
            MessageType = NetMessageType.CreateSeatResponse;
        }

        public Seat seat { get; set; }
    }

    public partial class UpdateSeatRequest : NetMessageBase
    {
        public UpdateSeatRequest()
        {
            MessageType = NetMessageType.UpdateSeatRequest;
        }

        public Seat seat { get; set; }
    }

    public partial class UpdateSeatResponse : NetMessageResponseBase
    {
        public UpdateSeatResponse()
        {
            MessageType = NetMessageType.UpdateSeatResponse;
        }

        public Seat seat { get; set; }
    }

    public partial class DeleteSeatRequest : NetMessageBase
    {
        public DeleteSeatRequest()
        {
            MessageType = NetMessageType.DeleteSeatRequest;
        }

        public Seat seat { get; set; }
    }

    public partial class DeleteSeatResponse : NetMessageResponseBase
    {
        public DeleteSeatResponse()
        {
            MessageType = NetMessageType.DeleteSeatResponse;
        }
    }

    public partial class CreatePackageRequest: NetMessageBase
    {
        public CreatePackageRequest()
        {
            MessageType = NetMessageType.CreatePackageRequest;
        }

        public PackageInfo packageInfo { get; set; }
    }

    public partial class CreatePackageResponse: NetMessageResponseBase
    {
        public CreatePackageResponse()
        {
            MessageType = NetMessageType.CreatePackageResponse;
        }
    }

    public partial class UpdatePackageRequest : NetMessageBase
    {
        public UpdatePackageRequest()
        {
            MessageType = NetMessageType.UpdatePackageRequest;
        }

        public PackageInfo packageInfo { get; set; }
    }

    public partial class UpdatePackageResponse : NetMessageResponseBase
    {
        public UpdatePackageResponse()
        {
            MessageType = NetMessageType.UpdatePackageResponse;
        }
    }

    public partial class DeletePackageRequest : NetMessageBase
    {
        public DeletePackageRequest()
        {
            MessageType = NetMessageType.DeletePackageRequest;
        }

        public PackageInfo packageInfo { get; set; }
    }

    public partial class DeletePackageResponse : NetMessageResponseBase
    {
        public DeletePackageResponse()
        {
            MessageType = NetMessageType.DeletePackageResponse;
        }
    }

    #endregion
}