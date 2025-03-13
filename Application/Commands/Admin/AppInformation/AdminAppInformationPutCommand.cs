using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminAppInformationPutCommand : IRequest<CommandResponse<AppInformation>>
    {
		public AdminAppInformationPutCommand()
		{
		}
        public string CompanyTitle { get; set; }
        public string CompanyIcon { get; set; }
    }
}

