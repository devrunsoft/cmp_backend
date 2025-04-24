using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class GetVehicleHandler : IRequestHandler<GetVehicleCommand, CommandResponse<Vehicle>>
    {
        private readonly IVehicleRepository _repository;
        public GetVehicleHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Vehicle>> Handle(GetVehicleCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.ProviderId == request.ProviderId,
                query => query.Include(x=>x.VehicleService)
                .Include(x => x.VehicleCompartment)
            )).FirstOrDefault();
            return new Success<Vehicle>() { Data = result };
        }
    }
}

