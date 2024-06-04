﻿using System;
using System.Linq;
using NBSSR.Network;
using NBSSRServer.Extensions;
using NBSSRServer.MiniDatabase;

namespace NBSSRServer.Services
{
    public class CreateSeatService : NBServiceBase<CreateSeatRequest, CreateSeatResponse>
    {
        public override CreateSeatResponse ProcessMessage(CreateSeatRequest request)
        {
            return CreateSeat(request);
        }

        private CreateSeatResponse CreateSeat(CreateSeatRequest request)
        {
            CreateSeatResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;
            Seat seat = request.seat;
            if (SeatService.GetSeat(seat.storeID, seat.floorID, seat.seatID) == null)
            {
                response.ErrorMsg = "seat already exist.";
                return response;
            }

            Floor floor = FloorService.GetFloor(seat.storeID, seat.floorID);
            if (floor == null)
            {
                response.ErrorMsg = "can not find target floor.";
                return response;
            }

            int seatID = floor.seats.Count + 1;
            seat.seatID = seatID;
            response.seat = seat;
            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"create seat success: {seat.Json()}");

            return response;
        }
    }

    public class UpdateSeatService : NBServiceBase<UpdateSeatRequest, UpdateSeatResponse>
    {
        public override UpdateSeatResponse ProcessMessage(UpdateSeatRequest request)
        {
            return UpdateSeat(request);
        }

        private UpdateSeatResponse UpdateSeat(UpdateSeatRequest request)
        {
            return null;
        }
    }

    public class SeatService : NBService
    {
        public static Seat GetSeat(int storeID, int floorID, int seatID)
        {
            Store store = StoreService.GetStore(storeID);
            if (store == null)
            {
                logger.LogError($"can not find target store: {storeID}");
                return null;
            }

            Floor floor = store.floors.Find(item => item.floorID == floorID);
            if (floor == null)
            {
                logger.LogError($"can not find target floor: {floorID}");
                return null;
            }

            Seat seat = floor.seats.Find(item => item.seatID == seatID);
            if (seat == null)
            {
                logger.LogError($"can not find target seat: {seatID}");
                return null;
            }

            return seat;
        }
    }
}
