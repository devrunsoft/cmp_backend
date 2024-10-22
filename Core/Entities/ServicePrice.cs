using System;
namespace CMPNatural.Core.Entities
{
	public partial class ProductPrice
	{
        public long Id { get; set; }

        public string Name { get; set; }

        public long ServiceId { get; set; }

        public string ServicePriceCrmId { get; set; }

        public string ServiceCrmId { get; set; }

    }
}

