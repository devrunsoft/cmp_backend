using System;
namespace CMPNatural.Core.Entities
{
    public partial class ProviderService
    {
        public ProviderService()
		{
		}
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long ProductId { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual Product Product { get; set; }
    }
}

