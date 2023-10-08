namespace NBSSR.Network
{
    public enum NetMessageType
    {
        TestRequest = -2,
        TestResponse = -1,
    }

    public class NetMessageBase
    {
        public NetMessageType MessageType { get; set; }
    }

    public partial class TestRequest : NetMessageBase
    {
        public TestRequest()
        {
            MessageType = NetMessageType.TestRequest;
        }
        
        public uint uid { get; set; }
        public string platform { get; set; }
    }
    
    public partial class TestResponse : NetMessageBase
    {
        public TestResponse()
        {
            MessageType = NetMessageType.TestResponse;
        }
        
        public bool state { get; set; }
    }
}