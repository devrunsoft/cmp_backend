using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;
using System;
using CMPNatural.Application.Commands.Admin.provider;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class AdminPostProviderHandler : IRequestHandler<AdminPostProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminPostProviderHandler(IProviderReposiotry providerReposiotry)
        {
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<Provider>> Handle(AdminPostProviderCommand request, CancellationToken cancellationToken)
        {
            var entity = new Provider()
            {
                Lat = request.Lat,
                Long = request.Long,
                Name = request.Name,
                Rating = request.Rating,
                Address = request.Address,
                County = request.County,
                City = request.City,
                Status = (int) request.Status
            };
            var result = await _providerReposiotry.AddAsync(entity);

            return new Success<Provider>() { Data = result };

        }
    }
}

