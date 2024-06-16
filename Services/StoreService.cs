using System;
using System.Linq;
using NBSSR.Network;
using NBSSRServer.Extensions;
using NBSSRServer.MiniDatabase;

namespace NBSSRServer.Services
{
    public class StoreService
    {
        public static Store GetStore(int storeID)
        {
            return MiniDataManager.Instance.storeDB.Get(item => item.storeID == storeID);
        }

        public static Store CreateStore(int id, string name, List<Floor> floors)
        {
            Store store = new Store();
            store.storeID = id;
            store.storeName = name;
            if (floors != null && floors.Count > 0)
            {
                store.floors = floors;
                for (int i = 0; i < store.floors.Count; i++)
                {
                    store.floors[i].storeID = store.storeID;
                }
            }

            return store;
        }
    }
}

