using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Command
{
    public class AdminRequestTerminatePostCommand : IRequest<CommandResponse<RequestTerminate>>
    {
        public long Id { get; set; }

    }
}

