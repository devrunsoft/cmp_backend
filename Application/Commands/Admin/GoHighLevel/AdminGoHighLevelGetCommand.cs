using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminGoHighLevelGetCommand : IRequest<CommandResponse<GoHighLevel>>
    {
		public AdminGoHighLevelGetCommand()
		{
		}
	}
}

