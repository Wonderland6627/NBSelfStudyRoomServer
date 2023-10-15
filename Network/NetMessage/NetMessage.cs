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

        public int a { get; set; }
        public int b { get; set; }
    }
    
    public partial class TestResponse : NetMessageBase
    {
        public TestResponse()
        {
            MessageType = NetMessageType.TestResponse;
        }
        
        public int c { get; set; }
    }
}