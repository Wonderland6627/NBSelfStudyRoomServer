using System;
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
            if (seat == null)
            {
                response.ErrorMsg = "empty param seat.";
                return response;
            }

            if (FloorService.GetFloor(seat.storeID, seat.floorID) == null)
            {
                response.ErrorMsg = "can not find target floor.";
                return response;
            }

            int seatID = 0;
            List<Seat> seats = SeatService.GetSeats(seat.storeID, seat.floorID);
            if (seats != null && seats.Count > 0)
            {
                seatID = seats.Count;
            }
            seat.seatID = seatID;

            MiniDataManager.Instance.seatDB.Add(seat);

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
            UpdateSeatResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;
            Seat seat = request.seat;
            if (seat == null)
            {
                response.ErrorMsg = "empty param seat.";
                return response;
            }

            if (SeatService.GetSeat(seat.storeID, seat.floorID, seat.seatID) == null)
            {
                response.ErrorMsg = "seat not exist.";
                return response;
            }

            MiniDataManager.Instance.seatDB.Update((item) =>
            {
                return item.storeID == seat.storeID && item.floorID == seat.floorID && item.seatID == seat.seatID;
            }, seat);

            response.seat = seat;
            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"update seat success: {seat.Json()}");

            return response;
        }
    }

    public class DeleteSeatService : NBServiceBase<DeleteSeatRequest, DeleteSeatResponse>
    {
        public override DeleteSeatResponse ProcessMessage(DeleteSeatRequest request)
        {
            return DeleteSeat(request);
        }

        private DeleteSeatResponse DeleteSeat(DeleteSeatRequest request)
        {
            DeleteSeatResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;
            Seat seat = request.seat;
            if (seat == null)
            {
                response.ErrorMsg = "empty param seat.";
                return response;
            }

            if (SeatService.GetSeat(seat.storeID, seat.floorID, seat.seatID) == null)
            {
                response.ErrorMsg = "seat not exist.";
                return response;
            }

            MiniDataManager.Instance.seatDB.Remove((item) =>
            {
                return item.storeID == seat.storeID && item.floorID == seat.floorID && item.seatID == seat.seatID;
            });

            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"delete seat success: {seat.Json()}");

            return response;
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

            Seat seat = MiniDataManager.Instance.seatDB.Get((item) =>
            {
                return item.storeID == storeID && item.floorID == floorID && item.seatID == seatID;
            });
            if (seat == null)
            {
                logger.LogError($"can not find target seat: {seatID}");
                return null;
            }

            return seat;
        }

        public static List<Seat> GetSeats(int storeID, int floorID)
        {
            return MiniDataManager.Instance.seatDB.datasList.FindAll((item) =>
            {
                return item.storeID == storeID && item.floorID == floorID;
            });
        }
    }
}

