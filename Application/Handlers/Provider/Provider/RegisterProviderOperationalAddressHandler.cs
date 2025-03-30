using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using System.Linq;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class RegisterProviderOperationalAddressHandler : IRequestHandler<RegisterProviderOperationalAddressCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _repository;

        public RegisterProviderOperationalAddressHandler(IProviderReposiotry repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Provider>> Handle(RegisterProviderOperationalAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(x=>x.Id == request.ProviderId && x.Status != ProviderStatus.Blocked)).FirstOrDefault();

            List<ProviderService> providerServices = new List<ProviderService>();
            foreach (var item in request.ProductIds)
            {
                providerServices.Add(new ProviderService() { ProductId = item });
            }


            entity.Lat = request.Lat;
            entity.Long = request.Long;
            entity.City = request.City;
            entity.Address = request.Address;
            entity.County = request.County;
            entity.AreaLocation = request.AreaLocation;
            entity.RegistrationStatus = ProviderRegistrationStatus.Operational_Address;
            entity.ProviderService = providerServices;

            await _repository.UpdateAsync(entity);

            return new Success<Provider>() { Data = entity };
        }
    }
}

