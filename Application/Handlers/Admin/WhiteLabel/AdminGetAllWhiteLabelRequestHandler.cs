using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminGetAllWhiteLabelRequestHandler : IRequestHandler<AdminGetAllWhiteLabelRequestCommand, CommandResponse<PagesQueryResponse<WhiteLabelRequest>>>
    {
        private readonly IWhiteLabelRequestRepository _repository;

        public AdminGetAllWhiteLabelRequestHandler(IWhiteLabelRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<WhiteLabelRequest>>> Handle(AdminGetAllWhiteLabelRequestCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetBasePagedAsync(
                request,
                x =>
                    (!request.ProviderId.HasValue || x.ProviderId == request.ProviderId.Value) &&
                    (!request.TenantId.HasValue || x.TenantId == request.TenantId.Value) &&
                    (!request.WantsDispatchManagement.HasValue || x.WantsDispatchManagement == request.WantsDispatchManagement.Value) &&
                    (!request.WantsCustomDomain.HasValue || x.WantsCustomDomain == request.WantsCustomDomain.Value) &&
                    (!request.WantsWhiteLabelBranding.HasValue || x.WantsWhiteLabelBranding == request.WantsWhiteLabelBranding.Value) &&
                    (!request.Status.HasValue || x.Status == request.Status.Value),
                query => query
                    .Include(x => x.Provider)
                    .Include(x => x.Tenant),
                filterAll: false);

            return new Success<PagesQueryResponse<WhiteLabelRequest>>() { Data = result };
        }
    }
}
