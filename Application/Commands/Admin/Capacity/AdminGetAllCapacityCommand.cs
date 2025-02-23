using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class AdminGetAllCapacityCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<Capacity>>>
    {
		public AdminGetAllCapacityCommand()
		{
		}

		public bool? Enable { get; set; }
	}
}

