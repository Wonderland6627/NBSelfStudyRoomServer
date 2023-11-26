using NBSSR.Network;
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

        public MiniDatabase<UserInfo> userInfoDB = new(GetDBPath("User"));

        private static readonly string DBDirName = "DB";
        private static readonly string DBSuffix = ".nbssrdb";

        private static string GetDBPath(string dbName)
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
