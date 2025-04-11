using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using System.Linq;
using System.Collections.Generic;
using CMPNatural.Application.Commands.Provider.Biling;

namespace CMPNatural.Application
{
    public class ProviderGetBilingHandler : IRequestHandler<ProviderGetBilingCommand, CommandResponse<BillingInformationProvider>>
    {
        private readonly IBillingInformationProviderRepository _repository;
        public ProviderGetBilingHandler(IBillingInformationProviderRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<BillingInformationProvider>> Handle(ProviderGetBilingCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(x => x.ProviderId == request.ProviderId)).LastOrDefault();

            return new Success<BillingInformationProvider>() { Data = entity };
        }
    }
}

