using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace CMPNatural.Application
{

    public class GetAllVehicleHandler : IRequestHandler<GetAllVehicleCommand, CommandResponse<List<Vehicle>>>
    {
        private readonly IVehicleRepository _repository;
        public GetAllVehicleHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<Vehicle>>> Handle(GetAllVehicleCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.ProviderId == request.ProviderId)).ToList();
            return new Success<List<Vehicle>>() { Data = result };
        }
    }
}

