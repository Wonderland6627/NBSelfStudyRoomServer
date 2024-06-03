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

            { NetMessageType.CreateStudentRequest, typeof(CreateStudentRequest) },
            { NetMessageType.CreateStudentResponse, typeof(CreateStudentResponse) },

            { NetMessageType.LoginRequest, typeof(LoginRequest) },
            { NetMessageType.LoginResponse, typeof(LoginResponse) },
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
        public static object Deserialize(string json, out string errorMsg)
        {
            JObject jObj = JObject.Parse(json);
            if (jObj == null)
            {
                errorMsg = "Parse JObject failed";
                return null;
            }

            JToken rawMessageType = jObj["MessageType"];
            if (rawMessageType == null)
            {
                errorMsg = "JObject does not have param ‘MessageType’";
                return null;
            }

            string typeRawValue = rawMessageType.Value<string>();
            NetMessageType netMessageType = (NetMessageType)Enum.Parse(typeof(NetMessageType), typeRawValue);
            if (!MessageType2CSTypesDic.TryGetValue(netMessageType, out Type objType))
            {
                errorMsg = $"{netMessageType} does not have value in MessageType2CSTypesDic";
                return null;
            }

            errorMsg = "Success";
            return jObj.ToObject(objType);
        }
    }
}