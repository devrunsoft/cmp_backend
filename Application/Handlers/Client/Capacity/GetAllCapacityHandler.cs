using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Commands;
using System.Linq;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class GetAllCapacityHandler : IRequestHandler<GetAllCapacityCommand, CommandResponse<List<Capacity>>>
    {
        private readonly ICapacityRepository _repository;
        public GetAllCapacityHandler(ICapacityRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }

        public async Task<CommandResponse<List<Capacity>>> Handle(GetAllCapacityCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _repository.GetAsync(p =>  p.Enable==true && p.ServiceType == request.ServiceType)).OrderBy(x => x.Order).ToList();
            return new Success<List<Capacity>>() { Data = invoices };
        }
    }
}

