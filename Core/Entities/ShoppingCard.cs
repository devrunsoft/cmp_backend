using System;
namespace CMPNatural.Core.Entities
{
	public partial class ShoppingCard
	{
        public long Id { get; set; }

        public string? ServicePriceCrmId { get; set; }

        public string? ServiceCrmId { get; set; }

        public long CompanyId { get; set; }

        public long OperationalAddressId { get; set; }

        public string? FrequencyType { get; set; }

        public string? Name { get; set; }

        public string? AddressName { get; set; }

        public string? PriceName { get; set; }

        public DateTime StartDate { get; set; }
    }
}

