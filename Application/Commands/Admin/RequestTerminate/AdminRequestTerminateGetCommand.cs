using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Command
{
	public class AdminRequestTerminateGetCommand : IRequest<CommandResponse<RequestTerminate>>
    {
		public long Id { get; set; }

	}
}

