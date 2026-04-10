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
    public class EditVehicleHandler : IRequestHandler<EditVehicleCommand, CommandResponse<Vehicle>>
    {
        private readonly IVehicleRepository _repository;
        private readonly IProviderVehicleRepository _providerVehicleRepository;

        public EditVehicleHandler(IVehicleRepository repository, IProviderVehicleRepository providerVehicleRepository)
        {
            _repository = repository;
            _providerVehicleRepository = providerVehicleRepository;
        }

        public async Task<CommandResponse<Vehicle>> Handle(EditVehicleCommand request, CancellationToken cancellationToken)
        {
            var existingVehicle = (await _repository.GetAsync(x => x.LicenseNumber == request.LicenseNumber && x.Id != request.Id)).Any();
            if (existingVehicle)
            {
                return new NoAcess<Vehicle>() { Message = "A vehicle with this license number already exists." };
            }

            var entity = (await _repository.GetAsync(
                x => x.Id == request.Id,
                query => query.Include(x => x.ProviderVehicle))).FirstOrDefault();

            if (entity == null)
            {
                return new NoAcess<Vehicle>() { Message = "Vehicle not found." };
            }

            var providerVehicle = entity.ProviderVehicle.FirstOrDefault(x => x.ProviderId == request.ProviderId && x.VehicleId == entity.Id);
            if (providerVehicle == null)
            {
                return new NoAcess<Vehicle>() { Message = "No Access to Edit!" };
            }

            entity.Capacity = request.Capacity;
            entity.LicenseNumber = request.LicenseNumber;
            if (!string.IsNullOrWhiteSpace(request.VehicleRegistration))
                entity.VehicleRegistration = request.VehicleRegistration;
            entity.VehicleRegistrationExp = request.VehicleRegistrationExp;
            if (!string.IsNullOrWhiteSpace(request.VehicleInsurance))
                entity.VehicleInsurance = request.VehicleInsurance;
            entity.VehicleInsuranceExp = request.VehicleInsuranceExp;
            if (!string.IsNullOrWhiteSpace(request.InspectionReport))
                entity.InspectionReport = request.InspectionReport;
            entity.InspectionReportExp = request.InspectionReportExp;
            entity.Picture = request.Picture;
            if (!string.IsNullOrWhiteSpace(request.MeasurementCertificate))
                entity.MeasurementCertificate = request.MeasurementCertificate;
            if (!string.IsNullOrWhiteSpace(request.PeriodicVehicleInspections))
                entity.PeriodicVehicleInspections = request.PeriodicVehicleInspections;
            entity.PeriodicVehicleInspectionsExp = request.PeriodicVehicleInspectionsExp;
            entity.Weight = request.Weight;
            entity.Name = request.Name;
            entity.ProviderId = request.ProviderId;

            await _repository.UpdateAsync(entity);

            return new Success<Vehicle>() { Data = entity };
        }
    }
}
