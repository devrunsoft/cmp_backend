using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;
using System.Collections.Generic;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminChangeStatusProviderHandler : IRequestHandler<AdminChangeStatusProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminChangeStatusProviderHandler(IProviderReposiotry providerReposiotry)
        {
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<Provider>> Handle(AdminChangeStatusProviderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _providerReposiotry.GetByIdAsync(request.ProviderId);

            entity.Status = request.Status;

            await _providerReposiotry.UpdateAsync(entity);

            return new Success<Provider>() { Data = entity };

        }
    }
}

