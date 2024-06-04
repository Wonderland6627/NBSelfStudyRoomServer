using System;
using NBSSR.Network;
using NBSSRServer.Extensions;
using NBSSRServer.MiniDatabase;

namespace NBSSRServer.Services
{
    public class CreateSeatService : NBServiceBase<CreateSeatRequest, CreateSeatResponse>
    {
        public override CreateSeatResponse ProcessMessage(CreateSeatRequest request)
        {
            return CreateSeat(request);
        }

        private CreateSeatResponse CreateSeat(CreateSeatRequest request)
        {
            return null;
        }
    }

    public class UpdateSeatService : NBServiceBase<UpdateSeatRequest, UpdateSeatResponse>
    {
        public override UpdateSeatResponse ProcessMessage(UpdateSeatRequest request)
        {
            return UpdateSeat(request);
        }

        private UpdateSeatResponse UpdateSeat(UpdateSeatRequest request)
        {
            return null;
        }
    }
}

