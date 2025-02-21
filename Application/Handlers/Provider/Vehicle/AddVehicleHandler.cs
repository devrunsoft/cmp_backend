using System;
using CMPNatural.Application.Services;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CMPNatural.Application
{

    public class AddVehicleHandler : IRequestHandler<AddVehicleCommand, CommandResponse<Vehicle>>
    {
        private readonly IVehicleRepository _repository;

        public AddVehicleHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Vehicle>> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
        {
            var path = Guid.NewGuid().ToString();
            var VehicleRegistration = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.VehicleRegistration, "VehicleRegistration", request.ProviderId, path);
            var VehicleInsurance = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.VehicleInsurance, "VehicleInsurance", request.ProviderId, path);
            var InspectionReport = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.InspectionReport, "InspectionReport", request.ProviderId, path);
            var Picture = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.Picture, "Picture", request.ProviderId, path);
            var MeasurementCertificate = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.MeasurementCertificate, "MeasurementCertificate", request.ProviderId, path);
            var PeriodicVehicleInspections = FileHandler.ProviderDriverfileHandler(request.BaseVirtualPath, request.PeriodicVehicleInspections, "PeriodicVehicleInspections", request.ProviderId, path);

            var entity = new Vehicle()
            {
                ProviderId = request.ProviderId,
                Capacity = request.Capacity,
                VehicleRegistration= VehicleRegistration,
                VehicleRegistrationExp = request.VehicleRegistrationExp,

                VehicleInsurance = VehicleInsurance,
                VehicleInsuranceExp = request.VehicleInsuranceExp,

                InspectionReport = InspectionReport,
                InspectionReportExp = request.InspectionReportExp,
                Picture= Picture,

                MeasurementCertificate = MeasurementCertificate,

                PeriodicVehicleInspections= PeriodicVehicleInspections,
                PeriodicVehicleInspectionsExp = request.PeriodicVehicleInspectionsExp,

                Weight = request.Weight,

                VehicleCompartment = request.VehicleCompartment,
                VehicleService = request.VehicleService,
                Name = request.Name
            };

            var result = await _repository.AddAsync(entity);

            return new Success<Vehicle>() { Data = result };
        }
    }
}

