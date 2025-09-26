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
            var VehicleRegistration = request.VehicleRegistration;
            var VehicleInsurance = request.VehicleInsurance;
            var InspectionReport = request.InspectionReport;
            var Picture = request.Picture;
            var MeasurementCertificate = request.MeasurementCertificate;
            var PeriodicVehicleInspections = request.PeriodicVehicleInspections;

            List<VehicleCompartment> vehicleCompartments = new List<VehicleCompartment>();
            for (int i = 0; i < request.VehicleCompartments.Count; i++)
            {
                var capacity = request.VehicleCompartments[i];
                if (capacity <= 0)
                {
                    return new NoAcess<Vehicle>()
                    {
                        Message = $"Compartment {i + 1} must have a capacity greater than 0."
                    };
                }

                vehicleCompartments.Add(new VehicleCompartment() { Capacity = capacity });
            }

            List<VehicleService> vehicleServices = new List<VehicleService>();
            foreach (var service in request.VehicleService)
            {
                if (service.Capacity <= 0)
                {
                    return new NoAcess<Vehicle>()
                    {
                        Message = $"The capacity for '{service.VehicleServiceStatus.GetDescription().Replace("_", " ")}' must be greater than 0."
                    };
                }

                vehicleServices.Add(new VehicleService()
                {
                    VehicleServiceStatus = service.VehicleServiceStatus,
                    Capacity = service.Capacity
                });
            }

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

                VehicleCompartment = vehicleCompartments,
                VehicleService = vehicleServices,
                Name = request.Name,
                CompartmentSize = request.VehicleCompartments.Count
            };

            var result = await _repository.AddAsync(entity);

            return new Success<Vehicle>() { Data = result };
        }
    }
}

