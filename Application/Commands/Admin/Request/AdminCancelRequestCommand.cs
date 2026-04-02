using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.Request
{
    public class AdminCancelRequestCommand : IRequest<CommandResponse<RequestEntity>>
    {
        public long RequestId { get; set; }
    }
}

