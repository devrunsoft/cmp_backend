using System;
using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class RegisterProviderDocumentCommand : IRequest<CommandResponse<Provider>>
    {
		public RegisterProviderDocumentCommand()
		{
		}
        public long ProviderId { get; set; }
        public string BaseVirtualPath { get; set; }

        public IFormFile BusinessLicense { get; set; }
        public DateTime? BusinessLicenseExp { get; set; }

        public IFormFile HealthDepartmentPermit { get; set; }
        public DateTime? HealthDepartmentPermitExp { get; set; }

        public IFormFile WasteHaulerPermit { get; set; }

        public IFormFile EPACompliance { get; set; }
        public DateTime? EPAComplianceExp { get; set; }

        public IFormFile Insurance { get; set; }
        public DateTime? InsuranceExp { get; set; }
    }
}

