using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class ShoppingCard
	{
        public long Id { get; set; }

        public string? ServicePriceCrmId { get; set; }

        public string? ServiceCrmId { get; set; }

        public long CompanyId { get; set; }

        public long? ProductPriceId { get; set; }

        public long? ProductId { get; set; }

        public long OperationalAddressId { get; set; }

        public string? FrequencyType { get; set; }

        public string? Name { get; set; }

        public string? AddressName { get; set; }

        public string? Address { get; set; }

        public string? PriceName { get; set; }

        public DateTime StartDate { get; set; }

        public int ServiceKind { get; set; }

        public int ServiceId { get; set; }

        public int Qty { get; set; }

        public string LocationCompanyIds { get; set; }

        public string DayOfWeek { get; set; } = "";

        public int FromHour { get; set; } = 480; //8 AM

        public int ToHour { get; set; } = 1080; //6 PM

    }
}

