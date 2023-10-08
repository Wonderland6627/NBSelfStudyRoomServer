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
        
        public static NetMessageBase Deserialize(string json, NetMessageType messageType)
        {
            if (!MessageType2CSTypesDic.TryGetValue(messageType, out Type objectType))
            {
                return null;
            }
            
            return JsonConvert.DeserializeObject(json, objectType) as NetMessageBase;
        }

        public static NetMessageBase Deserialize(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            string messageTypeString = (string)jsonObject["MessageType"];
            NetMessageType messageType = (NetMessageType)Enum.Parse(typeof(NetMessageType), messageTypeString);
            return Deserialize(json, messageType);
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}