using System;
using CMPNatural.Application.Commands.Admin.provider;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application.Handlers.Admin.provider
{
    public class AdminPutProviderHandler : IRequestHandler<AdminPutProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminPutProviderHandler(IProviderReposiotry providerReposiotry)
        {
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<Provider>> Handle(AdminPutProviderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _providerReposiotry.GetByIdAsync(request.Id);

            entity.Lat = request.Lat;
            entity.Long = request.Long;
            entity.Name = request.Name;
            entity.Rating = request.Rating;
            entity.Address = request.Address;
            entity.County = request.County;
            entity.City = request.City;
            entity.Status = (int)request.Status;

           await _providerReposiotry.UpdateAsync(entity);

           return new Success<Provider>() { Data = entity };

        }
    }
}

