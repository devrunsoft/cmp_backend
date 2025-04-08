using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Commands.Admin.ProviderServiceArea;
using System.Collections.Generic;
using CMPNatural.Core.Repositories;
using ScoutDirect.Core.Entities.Base;
using System.Linq;

namespace CMPNatural.Application
{

    public class AdminDeleteProviderServiceAreaHandler : IRequestHandler<AdminDeleteProviderServiceAreaCommand, CommandResponse<ServiceArea>>
    {
        private readonly IServiceAreaRepository _repository;
        public AdminDeleteProviderServiceAreaHandler(IServiceAreaRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<ServiceArea>> Handle(AdminDeleteProviderServiceAreaCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(p=>p.Id== request.Id && p.ProviderId == request.ProviderId)).FirstOrDefault();
            if (entity == null)
            {
                return new NoAcess<ServiceArea>() { };
            }

            await _repository.DeleteAsync(entity);
            return new Success<ServiceArea>() { Data = entity };
        }
    }

}