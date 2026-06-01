using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
    public partial class Tenant
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? SubDomain { get; set; }
        public string? Host { get; set; }
        public bool IsActive { get; set; } = true;
        public bool WantsDispatchManagement { get; set; }
        public bool HasAdminPortal { get; set; } = true;
        public bool HasClientPortal { get; set; } = true;
        public bool HasProviderPortal { get; set; } = true;
        public bool AdminCanViewAllRecords { get; set; }
        public bool AllowMainAdminAccess { get; set; } = true;
        //public bool AllowTenantAdminAccess { get; set; } = true;
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? LogoUrl { get; set; }
        public string? SupportEmail { get; set; }
        public bool Verified { get; set; }
        public long ProviderId { get; set; }
        public long WhiteLabelRequestId { get; set; }

        public virtual ICollection<TenantDomain> Domains { get; set; } = new List<TenantDomain>();
        public virtual ICollection<TenantAccess> TenantAccesses { get; set; } = new List<TenantAccess>();
        public virtual ICollection<AdminEntity> Admins { get; set; } = new List<AdminEntity>();
        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
        public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();
    }
}
