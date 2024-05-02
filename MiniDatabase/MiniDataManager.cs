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

        public MiniDatabase<AccountInfo> accountInfoDB = new(GetDBPath("Account"));
        public MiniDatabase<UserInfo> userInfoDB = new(GetDBPath("UserInfo"));

        private static readonly string DBDirName = "DB";
        private static readonly string DBSuffix = ".nbssrdb";

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
