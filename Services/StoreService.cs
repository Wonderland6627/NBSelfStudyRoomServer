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
    }
}

