using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin.Invoice;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminCheckLocationProviderHandler : IRequestHandler<AdminCheckOprLocationProviderCommand, CommandResponse<List<Provider>>>
    {
        private readonly IOperationalAddressRepository _locationCompanyRepository;
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminCheckLocationProviderHandler(IOperationalAddressRepository locationCompanyRepository, IProviderReposiotry providerReposiotry)
        {
            _locationCompanyRepository = locationCompanyRepository;
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<List<Provider>>> Handle(AdminCheckOprLocationProviderCommand request, CancellationToken cancellationToken)
        {
            //int MaxDistance = 2000; //meter
            var location = (await _locationCompanyRepository.GetByIdAsync(request.OperationalAddressId));
            var providers = (await _providerReposiotry.GetAllSearchProviderAsync(location.Lat, location.Long,
                p=>p.ProviderService.Any(p=>p.ProductId == request.ProductId))).ToList();
            return new Success<List<Provider>>() { Data = providers };
        }
    }
}

