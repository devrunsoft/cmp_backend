using System.Collections.Generic;

namespace CMPNatural.Core.Models
{
    public class TenantRequestContext
    {
        public long? TenantId { get; set; }
        public string? TenantName { get; set; }
        public string Host { get; set; } = string.Empty;
        public bool CanViewAllRecords { get; set; }
        public bool CanManageDispatch { get; set; }
        public List<long> AccessibleTenantIds { get; set; } = new();
        public bool IsResolvedFromDatabase { get; set; }
    }
}
