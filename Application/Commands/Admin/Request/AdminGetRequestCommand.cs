using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.Request
{
    public class AdminGetRequestCommand : IRequest<CommandResponse<RequestResponse>>
    {
        public AdminGetRequestCommand()
        {
        }
        public long RequestId { get; set; }
    }
}

