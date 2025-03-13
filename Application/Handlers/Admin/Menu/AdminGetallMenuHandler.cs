using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminGetallMenuHandler : IRequestHandler<AdminGetallMenuCommand, CommandResponse<List<Menu>>>
    {
        private readonly IMenuRepository _repository;

        public AdminGetallMenuHandler(IMenuRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }

        public async Task<CommandResponse<List<Menu>>> Handle(AdminGetallMenuCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAllAsync()).ToList();
            return new Success<List<Menu>>() { Data = entity };
        }
    }
}

