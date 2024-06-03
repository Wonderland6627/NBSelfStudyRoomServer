using NBSSR.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Services
{
    public class LoginService : NBServiceBase<LoginRequest, LoginResponse>
    {
        public override LoginResponse ProcessMessage(LoginRequest request)
        {
            return Login(request);
        }

        private LoginResponse Login(LoginRequest request)
        {
            return null;
        }
    }
}
