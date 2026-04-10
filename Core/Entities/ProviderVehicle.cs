using System;

namespace CMPNatural.Core.Entities
{
    public partial class ProviderVehicle
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long VehicleId { get; set; }

        public virtual Provider Provider { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
