using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using System.Linq;
using System.Collections.Generic;
using CMPNatural.Application.Command;

namespace CMPNatural.Application
{
    public class ProviderUpdateServiceAndLocationHandler : IRequestHandler<ProviderUpdateServiceAndLocationCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _repository;
        private readonly IProviderServiceRepository _providerServiceRepository;

        public ProviderUpdateServiceAndLocationHandler(IProviderReposiotry repository, IProviderServiceRepository providerServiceRepository)
        {
            _repository = repository;
            _providerServiceRepository = providerServiceRepository;
        }

        public async Task<CommandResponse<Provider>> Handle(ProviderUpdateServiceAndLocationCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(x => x.Id == request.ProviderId && x.Status != ProviderStatus.Blocked)).FirstOrDefault();

            var providerService = (await _providerServiceRepository.GetAsync(p => p.ProviderId == request.ProviderId)).ToList();
            if (providerService.Count > 0)
                await _providerServiceRepository.DeleteRangeAsync(providerService);

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
            entity.ManagerFirstName = request.ManagerFirstName;
            entity.ManagerLastName = request.ManagerLastName;
            entity.ManagerPhoneNumber = request.ManagerPhoneNumber;
            entity.RegistrationStatus = ProviderRegistrationStatus.Operational_Address;
            entity.ProviderService = providerServices;

            await _repository.UpdateAsync(entity);
            return new Success<Provider>() { Data = entity };
        }
    }
}

