namespace CMPNatural.Core.Entities
{
    public partial class TenantAccess
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public long AccessibleTenantId { get; set; }
        public bool CanViewRecords { get; set; }
        public bool CanManageDispatch { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual Tenant Tenant { get; set; } = null!;
        public virtual Tenant AccessibleTenant { get; set; } = null!;
    }
}
