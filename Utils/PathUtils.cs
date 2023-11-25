using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Utils
{
    public class PathUtils
    {
        /// <summary>
        /// 应用程序的目录路径，即应用程序所在的文件夹路径
        /// </summary>
        public static string GetApplicationDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
