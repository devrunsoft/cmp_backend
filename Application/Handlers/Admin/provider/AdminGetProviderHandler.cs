using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;
using CMPNatural.Application.Commands.Admin.provider;

namespace CMPNatural.Application
{

    public class AdminGetProviderHandler : IRequestHandler<AdminGetProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminGetProviderHandler(IProviderReposiotry providerReposiotry)
        {
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<Provider>> Handle(AdminGetProviderCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _providerReposiotry.GetByIdAsync(request.Id));

            return new Success<Provider>() { Data = invoices };

        }
    }
}

