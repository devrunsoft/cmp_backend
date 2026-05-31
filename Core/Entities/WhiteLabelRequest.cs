using System;
using System.Collections.Generic;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
    public partial class WhiteLabelRequest
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long? TenantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool ManageClientsDirectly { get; set; }

        public bool WantsClientPortal { get; set; }

        public bool WantsDispatchManagement { get; set; }

        public bool WantsInvoicing { get; set; }

        public bool WantsWhiteLabelBranding { get; set; }

        public bool WantsCustomDomain { get; set; }

        public string? CustomDomain { get; set; }

        public string? SubDomain { get; set; }

        public WhiteLabelStatus Status { get; set; }
        public string? AdminReviewNote { get; set; }



        public virtual Provider Provider { get; set; } = null!;
        public virtual Tenant? Tenant { get; set; }
    }
}
