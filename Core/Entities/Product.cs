using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = "";
        public string ProductType { get; set; }

        public int? Type {
            get {
                if (CollectionIds.Split(",").Contains("671826be39575cddc96abf50"))
                {
                    return (int)ProductCollection.Service;
                }
                if (CollectionIds.Split(",").Contains("671826d564d8053aafd1ca63"))
                {
                    return (int)ProductCollection.Product;
                }
                return null;
            }
        }

        public int? ServiceType
        {
            get
            {
                if (CollectionIds.Split(",").Contains("6720da386747d963d6dc64bd"))
                {
                    return (int)CMPNatural.Core.Enums.ServiceType.Cooking_Oil_Collection;
                }
                if (CollectionIds.Split(",").Contains("6720da4c6747d93389dc64de"))
                {
                    return (int)CMPNatural.Core.Enums.ServiceType.Grease_Trap_Management;
                }
                return null;
            }
        }

        public string CollectionIds { get; set; }
        public string ServiceCrmId { get; set; }
        public bool Enable { get; set; }

        public virtual ICollection<BaseServiceAppointment> ServiceAppointment { get; set; } = new List<BaseServiceAppointment>();
    }
}