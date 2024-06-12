using System;
using Newtonsoft.Json;

namespace NBSSRServer
{
	public static class CommonUtils
	{
		public static T Clone<T>(this T source)
		{
            if (source is null)
            {
                return default;
            }

            string jsonString = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
	}
}

