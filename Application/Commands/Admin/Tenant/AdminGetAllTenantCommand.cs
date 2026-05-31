using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetAllTenantCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<Tenant>>>
    {
        public bool? IsActive { get; set; }
        public bool? AllowMainAdminAccess { get; set; }
        public bool? AdminCanViewAllRecords { get; set; }
        public bool? WantsDispatchManagement { get; set; }
    }
}
