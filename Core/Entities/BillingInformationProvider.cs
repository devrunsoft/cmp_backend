using System;
namespace CMPNatural.Core.Entities
{
	public partial class BillingInformationProvider
	{
        public long Id { get; set; }
        public string Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZIPCode { get; set; }
        public long ProviderId { get; set; }
    }
}

