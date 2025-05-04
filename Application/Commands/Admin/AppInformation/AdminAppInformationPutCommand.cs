using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminAppInformationPutCommand : IRequest<CommandResponse<AppInformation>>
    {
		public AdminAppInformationPutCommand()
		{
		}
        public string CompanyTitle { get; set; }
        public IFormFile CompanyIcon { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyCeoLastName { get; set; }
        public string CompanyCeoFirstName { get; set; }
        public string Sign { get; set; }
        public string StripeApikey { get; set; }
        public string StripePaymentMethodConfiguration { get; set; }
        public string? BaseVirtualPath { get; set; } = ""; 
    }
}

