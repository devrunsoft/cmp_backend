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

    public class DriverGetAllVehicleHandler : IRequestHandler<DriverGetAllVehicleCommand, CommandResponse<List<Vehicle>>>
    {
        private readonly IVehicleRepository _repository;
        private readonly IDriverRepository driverRepository;
        public DriverGetAllVehicleHandler(IVehicleRepository repository, IDriverRepository driverRepository)
        {
            _repository = repository;
            this.driverRepository = driverRepository;
        }

        public async Task<CommandResponse<List<Vehicle>>> Handle(DriverGetAllVehicleCommand request, CancellationToken cancellationToken)
        {
            var driver = (await driverRepository.GetAsync(p => p.Id == request.DriverId)).FirstOrDefault();
            var result = (await _repository.GetAsync(p => p.ProviderId == driver.ProviderId)).ToList();
            return new Success<List<Vehicle>>() { Data = result };
        }
    }
}

