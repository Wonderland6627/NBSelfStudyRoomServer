﻿using NBSSR.Network;
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