using System;
using System.Linq;
using NBSSR.Network;
using NBSSRServer.Extensions;
using NBSSRServer.MiniDatabase;

namespace NBSSRServer.Services
{
    public class FloorService : NBService
    {
        public static Floor GetFloor(int storeID, int floorID)
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

            return floor;
        }
    }
}

