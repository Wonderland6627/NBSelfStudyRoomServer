using NBSSRServer.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Extensions
{
    public static class TypeExtensions
    {
        private static NBSSRLogger logger = new("TypeExtensions");

        public static string Json(this object obj)
        {
            if (obj == null) return "null";

            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (JsonException ex)
            {
                logger.LogError($"Failed to serialize object to JSON. Error: {ex.Message}");
                return "convert fail";
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while serializing object to JSON. Error: {ex}");
                return "convert fail";
            }
        }
    }
}
