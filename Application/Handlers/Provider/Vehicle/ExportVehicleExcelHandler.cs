using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Model;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class ExportVehicleExcelHandler : IRequestHandler<ExportVehicleExcelCommand, VehicleExcelFileResult>
    {
        private readonly IProviderVehicleRepository _providerVehicleRepository;

        public ExportVehicleExcelHandler(IProviderVehicleRepository providerVehicleRepository)
        {
            _providerVehicleRepository = providerVehicleRepository;
        }

        public async Task<VehicleExcelFileResult> Handle(ExportVehicleExcelCommand request, CancellationToken cancellationToken)
        {
            var vehicles = (await _providerVehicleRepository.GetAsync(
                x => x.ProviderId == request.ProviderId,
                query => query
                    .Include(x => x.Vehicle)
                    .ThenInclude(x => x.VehicleCompartment)
                    .Include(x => x.Vehicle)
                    .ThenInclude(x => x.VehicleService)))
                .Select(x => x.Vehicle)
                .ToList();

            return VehicleExcelService.Export(vehicles);
        }
    }
}
