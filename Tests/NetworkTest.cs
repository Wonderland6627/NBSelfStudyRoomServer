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
            if (seat == null)
            {
                return;
            }
            DeleteSeatRequest request = new DeleteSeatRequest();
            request.seat = seat;
            NetworkManager.Instance.OnReceiveMessageLocal(request);
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
    }
}