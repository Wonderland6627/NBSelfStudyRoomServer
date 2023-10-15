using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NBSSR.Network
{
    public class NetMsgSerializationHelper
    {
        public static Dictionary<NetMessageType, Type> MessageType2CSTypesDic = new Dictionary<NetMessageType, Type>
        {
            { NetMessageType.TestRequest, typeof(TestRequest) },
            { NetMessageType.TestResponse, typeof(TestResponse) },
        };

        public static Dictionary<Type, Type> MessagePairsDic = new Dictionary<Type, Type>
        {
            { typeof(TestRequest), typeof(TestResponse) },
        };

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string json)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}