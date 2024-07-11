using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NBSSRServer.Utils;
using System.Reflection;
using Newtonsoft.Json;
using NBSSRServer.Extensions;

namespace NBSSRServer.Tests
{
    internal class DataSourceTest
    {
        class location
        {
            public float lat, lng;
        }

        class schoolInfo
        {
            public string name;
            public string province;
            public string city;
            public string area;
            public string address;
            public location location;
        }

        public static void ReadAllData()
        {
            string path = $"{PathUtils.GetApplicationDirectory()}..\\..\\..\\/MiniDatabase/DataSource/SchoolInfo.json"; ;
            var sys = Environment.OSVersion;
            if (sys.Platform.ToString().Contains("Unix"))
            {
                //Mac
                path = $"{PathUtils.GetApplicationDirectory()}../../../MiniDatabase/DataSource/SchoolInfo.json";
            }

            var fullPath = Path.GetFullPath(path);
            var json = File.ReadAllText(fullPath);
            var datas = JsonConvert.DeserializeObject<List<schoolInfo>>(json);
            for (int i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                Console.WriteLine($"{data.province} {data.city} {data.area} {data.name}");
            }
            Console.Write("hi");
            Console.ReadKey();
        }


    }
}
