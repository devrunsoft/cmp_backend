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

        public string BusinessLicense { get; set; }
        public DateTime? BusinessLicenseExp { get; set; }

        public string HealthDepartmentPermit { get; set; }
        public DateTime? HealthDepartmentPermitExp { get; set; }

        public string WasteHaulerPermit { get; set; }

        public string EPACompliance { get; set; }
        public DateTime? EPAComplianceExp { get; set; }

        public string Insurance { get; set; }
        public DateTime? InsuranceExp { get; set; }
    }
}

