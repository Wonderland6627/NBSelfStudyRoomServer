using NBSSR.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Services
{
    public class TestService : NBServiceBase<TestRequest, TestResponse>
    {
        public override TestResponse ProcessMessage(TestRequest request)
        {
            if (request == null)
            {
                return null;
            }

            var response = new TestResponse();
            response.c = request.a + request.b;
            return response;
        }
    }
}
