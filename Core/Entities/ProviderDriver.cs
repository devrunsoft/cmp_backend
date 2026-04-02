using System;
namespace CMPNatural.Core.Entities
{
	public partial class ProviderDriver
	{
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long DriverId { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual Provider Provider { get; set; }
    }
}

