using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Command
{
	public class AdminRequestTerminateGetAllCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<RequestTerminate>>>
    {
		public AdminRequestTerminateGetAllCommand()
		{
		}
	}
}

