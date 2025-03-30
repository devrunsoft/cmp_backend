using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class DeleteLocationCommand : IRequest<CommandResponse<object>>
    {
        public DeleteLocationCommand()
        {
        }
        public long Id { get; set; }
        public long CompanyId { get; set; }

    }
}

