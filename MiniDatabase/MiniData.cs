using Newtonsoft.Json;

namespace NBSSRServer.MiniDatabase
{
    public class MiniData<T> where T : class
    {
        public DateTime createdTime;
        public DateTime updatedTime;
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
