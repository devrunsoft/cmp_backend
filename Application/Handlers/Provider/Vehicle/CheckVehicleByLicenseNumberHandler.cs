using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class CheckVehicleByLicenseNumberHandler : IRequestHandler<CheckVehicleByLicenseNumberCommand, CommandResponse<List<Vehicle>>>
    {
        private readonly IVehicleRepository _repository;

        public CheckVehicleByLicenseNumberHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<Vehicle>>> Handle(CheckVehicleByLicenseNumberCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.LicenseNumber.Contains(request.LicenseNumber))).ToList();
            return new Success<List<Vehicle>>() { Data = result };
        }
    }
}
