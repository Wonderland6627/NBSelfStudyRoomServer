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
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 从消息体中取出MessageType 找到对应的Type 反序列化
        /// </summary>
        public static object Deserialize(string json)
        {
            JObject jObj = JObject.Parse(json);
            if (jObj == null)
            {
                return null;
            }

            JToken rawMessageType = jObj["MessageType"];
            if (rawMessageType == null)
            {
                return null;
            }

            int typeRawValue = rawMessageType.Value<int>();
            NetMessageType netMessageType = (NetMessageType)Enum.Parse(typeof(NetMessageType), typeRawValue.ToString());
            if (!MessageType2CSTypesDic.TryGetValue(netMessageType, out Type objType))
            {
                return null;
            }

            return jObj.ToObject(objType);
        }
    }
}