using NBSSR.Network;
using NBSSRServer.Extensions;
using NBSSRServer.Network;
using NBSSRServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Tests
{
    internal class NetworkTest
    {
        public static void CreateStuentInfoTest()
        {
            StudentInfo info = new();
            info.name = Guid.NewGuid().ToString();
            info.phone = info.name;
            info.userType = UserType.Student;

            CreateStudentInfoRequest request = new();
            request.studentInfo = info;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void GetStudentInfoTest()
        {
            GetStudentInfoRequest request = new();
            request.studentID = 0;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void UpdateStudentInfoTest()
        {
            StudentInfo info = new();
            info.userID = 0;
            info.name = "123test";
            info.phone = "123456789";

            UpdateStudentInfoRequest request = new();
            request.studentInfo = info;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void DeleteStudentInfoTest()
        {
            DeleteStudentInfoRequest request = new();
            request.studentID = 0;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void CreateSeatTest()
        {
            Seat seat = new();
            seat.storeID = -1;
            seat.floorID = 0;
            seat.seatName = "测试座位";
            seat.position = new Position(1, 1);
            seat.seatType = SeatType.C;
            seat.studentID = 0;
            seat.isOccupied = false;

            CreateSeatRequest request = new CreateSeatRequest();
            request.seat = seat;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void UpdateSeatTest()
        {
            Seat seat = SeatService.GetSeat(-1, 0, 0);
            if (seat == null)
            {
                return;
            }
            seat.position = new Position(2, 2);
            UpdateSeatRequest request = new UpdateSeatRequest();
            request.seat = seat;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void DeleteSeatTest()
        {
            Seat seat = SeatService.GetSeat(-1, 0, 1);
            DeleteSeatRequest request = new DeleteSeatRequest();
            request.seat = seat;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void CreatePackageTest()
        {
            //创建 C座位 月卡 套餐
            PackageInfo packageInfo = new();
            packageInfo.seatType = SeatType.C;
            packageInfo.packageType = PackageType.Month;
            packageInfo.price = 700;
            packageInfo.discount = 0;
            packageInfo.giftDayCount = 3;
            CreatePackageRequest request = new();
            request.packageInfo = packageInfo;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void UpdatePackageTest()
        {
            PackageInfo packageInfo = PackageService.GetPackageInfo(SeatType.C, PackageType.Month);
            packageInfo.price = 800;
            UpdatePackageRequest request = new();
            request.packageInfo = packageInfo;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void DeletePackageTest()
        {
            PackageInfo packageInfo = PackageService.GetPackageInfo(SeatType.C, PackageType.Month);
            DeletePackageRequest request = new();
            request.packageInfo = packageInfo;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
        }

        public static void RequestTest()
        {
            TestRequest request = new();
            request.a = 10;
            request.b = 20;
            NetworkManager.Instance.SendToSelf(request);
        }
    }
}

namespace NBSSRServer.Network
{
    internal partial class NetworkManager
    {
        public void OnReceiveMessageLocal(NetMessageBase request)
        {
            logger.LogInfo($"Server receive local message for test: {request.Json()}");
            OnReceiveMessage(request, (rspObj) =>
            {
                logger.LogInfo($"Server response local message for test: {rspObj.Json()}");
            });
        }

        public async void SendToSelf(NetMessageBase request)
        {
            using (HttpClient client = new())
            {
                string json = request.Json();
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("platform", "serverself");
                HttpResponseMessage responseMessage = await client.PostAsync(Program.HttpUrl, content);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    Console.WriteLine("HTTP request failed with status code: " + responseMessage.StatusCode);
                    return;
                }
                string responseJson = await responseMessage.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseJson);
            }
        }
    }
}