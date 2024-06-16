using NBSSR.Network;
using NBSSRServer.Extensions;
using NBSSRServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Tests
{
    internal class NetworkTest
    {
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