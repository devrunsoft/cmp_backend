using System;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Xml.Linq;

namespace CMPNatural.Application
{
    public class EditVehicleHandler : IRequestHandler<EditVehicleCommand, CommandResponse<Vehicle>>
    {
        private readonly IVehicleRepository _repository;

        public EditVehicleHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Vehicle>> Handle(EditVehicleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity.ProviderId != request.ProviderId)
            {
                return new NoAcess<Vehicle>() { Message = "No Access to Edit!" };
            }
            string VehicleRegistration = null;
            string VehicleInsurance = null;
            string InspectionReport = null;
            string Picture = null;
            string MeasurementCertificate = null;
            string PeriodicVehicleInspections = null;

            var path = Guid.NewGuid().ToString();

            if (request.VehicleRegistration != null)
                VehicleRegistration = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.VehicleRegistration, "VehicleRegistration", request.ProviderId, path);
            if (request.VehicleInsurance != null)
                VehicleInsurance = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.VehicleInsurance, "VehicleInsurance", request.ProviderId, path);
            if (request.InspectionReport != null)
                InspectionReport = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.InspectionReport, "InspectionReport", request.ProviderId, path);
            if (request.Picture != null)
                Picture = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.Picture, "Picture", request.ProviderId, path);
            if (request.MeasurementCertificate != null)
                MeasurementCertificate = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.MeasurementCertificate, "MeasurementCertificate", request.ProviderId, path);
            if (request.PeriodicVehicleInspections != null)
                PeriodicVehicleInspections = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.PeriodicVehicleInspections, "PeriodicVehicleInspections", request.ProviderId, path);

            entity.Cap = request.Capacity;
            if (VehicleRegistration != null)
                entity.VehicleRegistration = VehicleRegistration;
            entity.VehicleRegistrationExp = request.VehicleRegistrationExp;
            if (VehicleInsurance != null)
                entity.VehicleInsurance = VehicleInsurance;
            entity.VehicleInsuranceExp = request.VehicleInsuranceExp;
            if (InspectionReport != null)
                entity.InspectionReport = InspectionReport;
            entity.InspectionReportExp = request.InspectionReportExp;
            entity.Picture = Picture;
            if (MeasurementCertificate != null)
                entity.MeasurementCertificate = MeasurementCertificate;
            if (PeriodicVehicleInspections != null)
                entity.PeriodicVehicleInspections = PeriodicVehicleInspections;
            entity.PeriodicVehicleInspectionsExp = request.PeriodicVehicleInspectionsExp;
            entity.Weight = request.Weight;
            entity.Name = request.Name;

             await _repository.UpdateAsync(entity);

            return new Success<Vehicle>() { Data = entity };
        }
    }
}

