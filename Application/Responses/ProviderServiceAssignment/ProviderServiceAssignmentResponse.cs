using System;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Responses.ProviderServiceAssignment
{
	public class ProviderServiceAssignmentResponse
	{
		public ProviderServiceAssignmentResponse()
		{
		}

        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long InvoiceId { get; set; }
        public long CompanyId { get; set; }
        public int Status { get; set; }
        public DateTime AssignTime { get; set; }
        public InvoiceResponse Invoice { get; set; }
        public CompanyResponse Company { get; set; }
    }
}

