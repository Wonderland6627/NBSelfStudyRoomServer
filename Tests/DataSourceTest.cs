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
using NBSSRServer.MiniDatabase;

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
            //public string address;
            //public location location;
        }

        public static void ReadAllData()
        {
            string path = $"{PathUtils.GetApplicationDirectory()}..\\..\\..\\/MiniDatabase/DataSource/SchoolInfo.json";
            var sys = Environment.OSVersion;
            if (sys.Platform.ToString().Contains("Unix")) //Mac
            {
                path = $"{PathUtils.GetApplicationDirectory()}../../../MiniDatabase/DataSource/SchoolInfo.json";
            }

            var fullPath = Path.GetFullPath(path);
            var json = File.ReadAllText(fullPath);
            var datas = JsonConvert.DeserializeObject<List<schoolInfo>>(json);
            Console.WriteLine($"原始数据数量: {datas.Count}");
            datas = Distinct(datas);
            Console.WriteLine($"去重后数据数量: {datas.Count}");

            Save(datas);


            Console.Write("hi");
            Console.ReadKey();
        }

        static List<schoolInfo> Distinct(List<schoolInfo> datas)
        {
            HashSet<string> keysSet = new();
            List<schoolInfo> results = new();
            for (int i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                var key = $"{data.province}-{data.city}-{data.area}-{data.name}";
                if (keysSet.Contains(key))
                {
                    //Console.WriteLine(key);
                    continue;
                }

                keysSet.Add(key);
                results.Add(data);
            }

            return results;
        }

        static void Save(List<schoolInfo> datas)
        {
            var saveDirPath = $"{PathUtils.GetApplicationDirectory()}..\\..\\..\\/Tests/DataSource";
            var sys = Environment.OSVersion;
            if (sys.Platform.ToString().Contains("Unix")) //Mac
            {
                saveDirPath = $"{PathUtils.GetApplicationDirectory()}../../../Tests/DataSource";
            }
            var saveDistinctPath = Path.Combine(saveDirPath, "DistinctSchoolInfo.json");

            Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> schoolInfosDic = new();
            for (int i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                if (!schoolInfosDic.ContainsKey(data.province))
                {
                    schoolInfosDic[data.province] = new();
                }
                if (!schoolInfosDic[data.province].ContainsKey(data.city))
                {
                    schoolInfosDic[data.province][data.city] = new();
                }
                if (!schoolInfosDic[data.province][data.city].ContainsKey(data.area))
                {
                    schoolInfosDic[data.province][data.city][data.area] = new();
                }
                schoolInfosDic[data.province][data.city][data.area].Add(data.name);
            }

            Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> dic = new();
            List<string> schools = new List<string>() { "1", "2", "3" };
            Dictionary<string, List<string>> area = new();
            area["schools"] = schools;
            Dictionary<string, Dictionary<string, List<string>>> city = new();
            city["area"] = area;
            dic["provice"] = city;

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            var json = JsonConvert.SerializeObject(schoolInfosDic, settings);

            FileStreamOptions fso = new();
            fso.Access = FileAccess.ReadWrite;
            fso.Mode = FileMode.OpenOrCreate;
            using (StreamWriter sw = new StreamWriter(saveDistinctPath, fso))
            {
                sw.BaseStream.SetLength(0);
                sw.Write(json);
                sw.Flush();
            }
        }
    }
}
