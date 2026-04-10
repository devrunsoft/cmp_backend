using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class GetVehicleHandler : IRequestHandler<GetVehicleCommand, CommandResponse<Vehicle>>
    {
        private readonly IProviderVehicleRepository _repository;
        public GetVehicleHandler(IProviderVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Vehicle>> Handle(GetVehicleCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(
                p => p.ProviderId == request.ProviderId && p.VehicleId == request.Id,
                query => query
                    .Include(x => x.Vehicle)
                    .ThenInclude(x => x.VehicleService)
                    .Include(x => x.Vehicle)
                    .ThenInclude(x => x.VehicleCompartment)))
                .Select(x => x.Vehicle)
                .FirstOrDefault();
            return new Success<Vehicle>() { Data = result };
        }
    }
}
