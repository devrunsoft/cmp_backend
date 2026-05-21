using System;

namespace CMPNatural.Core.Entities
{
    public class WhiteLabelRequest
    {
        public bool ManageClientsDirectly { get; set; }

        public bool WantsClientPortal { get; set; }

        public bool WantsDispatchManagement { get; set; }

        public bool WantsInvoicing { get; set; }

        public bool WantsWhiteLabelBranding { get; set; }

        public bool WantsCustomDomain { get; set; }

        public string? CustomDomain { get; set; }

        public string? SubDomain { get; set; }
    }
}