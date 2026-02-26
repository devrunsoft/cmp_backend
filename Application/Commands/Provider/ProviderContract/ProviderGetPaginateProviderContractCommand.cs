using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Base;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class ProviderGetPaginateProviderContractCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<ProviderContract>>>
    {
        public ProviderGetPaginateProviderContractCommand()
        {
        }
        public CompanyContractStatus? Status { get; set; }
        public long? ProviderId { get; set; }
    }
}

