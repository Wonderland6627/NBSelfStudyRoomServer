using NBSSR.Network;
using NBSSRServer.Services;
using NBSSRServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.MiniDatabase
{
    public class MiniDataManager
    {
        private static MiniDataManager instance;
        public static MiniDataManager Instance
        {
            get 
            {
                instance = instance ?? new MiniDataManager();

                return instance; 
            }
        }  

        public MiniDataManager() { }

        public MiniDatabase<Store> storeDB = new(GetDBPath("Store"));
        public MiniDatabase<Seat> seatDB = new(GetDBPath("Seat"));

        public MiniDatabase<AccountInfo> accountInfoDB = new(GetDBPath("AccountInfo"));
        public MiniDatabase<StudentInfo> studentInfoDB = new(GetDBPath("StudentInfo"));

        public MiniDatabase<PackageInfo> packageInfoDB = new(GetDBPath("PackageInfo"));

        private static readonly string DBDirName = "DB";
        private static readonly string DBSuffix = ".nbssrdb";

        public void Init()
        {
            SetupCurrentStores();
        }

        /// <summary>
        /// 目前只有一个门店，简便化处理，直接在服务中初始化
        /// 若不存在则在服务启动前对门店和楼层信息初始化
        /// </summary>
        public static void SetupCurrentStores()
        {
            Floor testFloor1 = FloorService.CreateFloor(0, "测试店-测试1层", new Size(20, 20));
            Floor testFloor2 = FloorService.CreateFloor(1, "测试店-测试2层", new Size(15, 15));
            List<Floor> testFloors = new List<Floor>
            {
                testFloor1,
                testFloor2,
            };

            Store testStore = StoreService.CreateStore(-1, "测试店", testFloors);
            Store mainStore = StoreService.CreateStore(0, "钢高总店", null);
            List<Store> stores = new List<Store>
            {
                testStore,
                mainStore,
            };

            for (int i = 0; i < stores.Count; i++)
            {
                Store store = stores[i];
                Store dbStore = MiniDataManager.Instance.storeDB.Get(item => item.storeID == store.storeID);
                if (dbStore == null)
                {
                    MiniDataManager.Instance.storeDB.Add(store);
                }
            }
        }

        public static string GetDBPath(string dbName)
        {
            string dbDirPath = Path.Combine(PathUtils.GetApplicationDirectory(), DBDirName);
            if (!Directory.Exists(dbDirPath))
            {
                Directory.CreateDirectory(dbDirPath);
            }

            return $"{Path.Combine(dbDirPath, dbName)}{DBSuffix}";
        }
    }
}
