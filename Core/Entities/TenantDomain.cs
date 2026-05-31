using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
    public partial class TenantDomain
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public string Host { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; } = true;
        public PortalType PortalType { get; set; }
        public bool Verified { get; set; }

        public virtual Tenant Tenant { get; set; } = null!;
    }
}
