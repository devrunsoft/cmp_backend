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
        public string CompanyAddress { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyCeoLastName { get; set; }
        public string CompanyCeoFirstName { get; set; }
        public string Sign { get; set; }
    }
}

