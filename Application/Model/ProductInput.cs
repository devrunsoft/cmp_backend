using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model
{
	public class ProductInput
	{
        public string Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public string ProductType { get; set; } = "";
        public ServiceType? Type { get; set; }
        public ProductCollection? ServiceType { get; set; }
        //public bool IsEmergency { get; set; }
        public string CollectionIds { get; set; } = "";
        public string ServiceCrmId { get; set; } = "";
        public bool Enable { get; set; }
        public int Order { get; set; }
    }
}

