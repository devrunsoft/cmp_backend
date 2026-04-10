using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class DeleteVehicleOfProviderHandler : IRequestHandler<DeleteVehicleOfProviderCommand, CommandResponse<Vehicle>>
    {
        private readonly IProviderVehicleRepository _repository;

        public DeleteVehicleOfProviderHandler(IProviderVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Vehicle>> Handle(DeleteVehicleOfProviderCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(
                p => p.VehicleId == request.VehicleId && p.ProviderId == request.ProviderId,
                query => query.Include(x => x.Vehicle))).FirstOrDefault();

            if (result == null)
            {
                return new NoAcess<Vehicle>() { Message = "Vehicle relation not found." };
            }

            await _repository.DeleteAsync(result);
            return new Success<Vehicle>() { Data = result.Vehicle };
        }
    }
}
