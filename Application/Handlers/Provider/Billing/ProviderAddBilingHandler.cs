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
    public class ProviderAddBilingHandler : IRequestHandler<ProviderAddBilingCommand, CommandResponse<BillingInformationProvider>>
    {
        private readonly IBillingInformationProviderRepository _repository;

        public ProviderAddBilingHandler(IBillingInformationProviderRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<BillingInformationProvider>> Handle(ProviderAddBilingCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(x=>x.Id == request.ProviderId)).LastOrDefault();
            if (entity == null)
            {

                entity = new BillingInformationProvider() {
                    Address = request.Address,
                    State = request.State,
                    ZIPCode = request.ZIPCode,
                    City = request.City,
                    ProviderId = request.ProviderId
            };
                entity= await _repository.AddAsync(entity);
            }
            else
            {
                entity.Address = request.Address;
                entity.State = request.State;
                entity.ZIPCode = request.ZIPCode;
                entity.City = request.City;
                await _repository.UpdateAsync(entity);
            }

            return new Success<BillingInformationProvider>() { Data = entity };
        }
    }
}

