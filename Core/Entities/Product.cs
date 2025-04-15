using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Product
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public string ProductType { get; set; } = "";

        public int? Type { get; set; }
        public int? ServiceType { get; set; }

        public bool IsEmergency { get {
                return this.Type == (int)ProductCollection.ServiceEmergency;
            } }
        public string? CollectionIds { get; set; } = "";
        public string? ServiceCrmId { get; set; } = "";
        public bool Enable { get; set; }
        public int Order { get; set; }

        public virtual ICollection<ProviderService> ProviderService { get; set; } = new List<ProviderService>();
        public virtual ICollection<ProductPrice> ProductPrice { get; set; } = new List<ProductPrice>();
        public virtual ICollection<BaseServiceAppointment> ServiceAppointment { get; set; } = new List<BaseServiceAppointment>();
    }
}
