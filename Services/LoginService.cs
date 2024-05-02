using NBSSR.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Services
{
    public class LoginService : NBServiceBase<LoginRequest, LoginRequest>
    {
        public override LoginRequest ProcessMessage(LoginRequest request)
        {
            return null;
        }
    }
}
