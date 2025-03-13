using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminAppInformationGetCommand : IRequest<CommandResponse<AppInformation>>
    {
		public AdminAppInformationGetCommand()
		{
		}
	}
}

