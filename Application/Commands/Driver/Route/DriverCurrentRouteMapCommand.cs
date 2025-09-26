using CMPNatural.Application.Responses.Driver;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class DriverCurrentRouteMapCommand : IRequest<CommandResponse<RouteDateResponse>>
    {
		public DriverCurrentRouteMapCommand()
		{
		}
		public long DriverId { get; set; }
	}
}

