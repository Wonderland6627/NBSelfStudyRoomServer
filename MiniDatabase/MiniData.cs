using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.MiniDatabase
{
    public class MiniData<T> where T : class
    {
        public DateTime updateTime;
        public List<T> datasList;

        public static MiniData<T> Build(string json)
        {
            return JsonConvert.DeserializeObject<MiniData<T>>(json);
        }

        public static string Build(MiniData<T> miniData)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            return JsonConvert.SerializeObject(miniData, settings);
        }
    }
}
