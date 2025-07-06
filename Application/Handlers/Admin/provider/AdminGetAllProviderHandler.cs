using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetAllProviderHandler : IRequestHandler<AdminGetAllProviderCommand, CommandResponse<PagesQueryResponse<Provider>>>
    {
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminGetAllProviderHandler(IProviderReposiotry providerReposiotry)
        {
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<PagesQueryResponse<Provider>>> Handle(AdminGetAllProviderCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _providerReposiotry.GetBasePagedAsync(request, p=> (request.Status == null || p.Status == request.Status), null));
            return new Success<PagesQueryResponse<Provider>>() { Data = invoices };
        }
    }
}

