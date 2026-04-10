using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class DriverGetAllVehicleHandler : IRequestHandler<DriverGetAllVehicleCommand, CommandResponse<List<Vehicle>>>
    {
        private readonly IProviderVehicleRepository _providerVehicleRepository;
        private readonly IDriverRepository _driverRepository;

        public DriverGetAllVehicleHandler(IProviderVehicleRepository providerVehicleRepository, IDriverRepository driverRepository)
        {
            _providerVehicleRepository = providerVehicleRepository;
            _driverRepository = driverRepository;
        }

        public async Task<CommandResponse<List<Vehicle>>> Handle(DriverGetAllVehicleCommand request, CancellationToken cancellationToken)
        {
            var driver = (await _driverRepository.GetAsync(
                p => p.Id == request.DriverId,
                query => query.Include(x => x.ProviderDriver))).FirstOrDefault();

            var providerIds = driver?.ProviderDriver?.Select(x => x.ProviderId).Distinct().ToList() ?? new List<long>();
            if (!providerIds.Any())
            {
                return new Success<List<Vehicle>>() { Data = new List<Vehicle>() };
            }

            var result = (await _providerVehicleRepository.GetAsync(
                p => providerIds.Contains(p.ProviderId),
                query => query.Include(x => x.Vehicle)))
                .Select(x => x.Vehicle)
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .ToList();

            return new Success<List<Vehicle>>() { Data = result };
        }
    }
}
