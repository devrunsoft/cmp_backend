using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Provider.WhiteLabel
{
    public class ProviderWhiteLabelRequestCommand : IRequest<CommandResponse<WhiteLabelRequest>>
    {
        public long ProviderId { get; set; }

        public bool ManageClientsDirectly { get; set; }

        public bool WantsClientPortal { get; set; }

        public bool WantsDispatchManagement { get; set; }

        public bool WantsInvoicing { get; set; }

        public bool WantsWhiteLabelBranding { get; set; }

        public bool WantsCustomDomain { get; set; }

        public string? CustomDomain { get; set; }

        public string? SubDomain { get; set; }

        public List<long> DispatchAccessibleTenantIds { get; set; } = new();
    }
}
