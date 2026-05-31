using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetAllWhiteLabelRequestCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<WhiteLabelRequest>>>
    {
        public long? ProviderId { get; set; }
        public long? TenantId { get; set; }
        public bool? WantsDispatchManagement { get; set; }
        public bool? WantsCustomDomain { get; set; }
        public bool? WantsWhiteLabelBranding { get; set; }
        public WhiteLabelStatus? Status { get; set; }
    }
}
