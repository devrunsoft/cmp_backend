using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AddVehicleHandler : IRequestHandler<AddVehicleCommand, CommandResponse<Vehicle>>
    {
        private readonly IVehicleRepository _repository;
        private readonly IProviderVehicleRepository _providerVehicleRepository;

        public AddVehicleHandler(IVehicleRepository repository, IProviderVehicleRepository providerVehicleRepository)
        {
            _repository = repository;
            _providerVehicleRepository = providerVehicleRepository;
        }

        public async Task<CommandResponse<Vehicle>> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
        {
            var existingVehicle = (await _repository.GetAsync(
                x => x.LicenseNumber == request.LicenseNumber,
                query => query
                    .Include(x => x.ProviderVehicle)
                    .Include(x => x.VehicleCompartment)
                    .Include(x => x.VehicleService))).FirstOrDefault();

            if (existingVehicle != null)
            {
                UpdateVehicle(existingVehicle, request);

                var relation = existingVehicle.ProviderVehicle
                    .FirstOrDefault(x => x.ProviderId == request.ProviderId && x.VehicleId == existingVehicle.Id);

                if (relation == null)
                {
                    existingVehicle.ProviderVehicle.Add(new ProviderVehicle
                    {
                        ProviderId = request.ProviderId,
                        VehicleId = existingVehicle.Id
                    });
                }

                await _repository.UpdateAsync(existingVehicle);
                return new Success<Vehicle>() { Data = existingVehicle };
            }

            var vehicleCompartments = BuildVehicleCompartments(request);
            var vehicleServices = BuildVehicleServices(request);
            if (vehicleCompartments == null || vehicleServices == null)
            {
                return new NoAcess<Vehicle>() { Message = _validationError! };
            }

            var entity = new Vehicle()
            {
                ProviderId = request.ProviderId,
                Capacity = request.Capacity,
                LicenseNumber = request.LicenseNumber,
                VehicleRegistration = request.VehicleRegistration,
                VehicleRegistrationExp = request.VehicleRegistrationExp,
                VehicleInsurance = request.VehicleInsurance,
                VehicleInsuranceExp = request.VehicleInsuranceExp,
                InspectionReport = request.InspectionReport,
                InspectionReportExp = request.InspectionReportExp,
                Picture = request.Picture,
                MeasurementCertificate = request.MeasurementCertificate,
                PeriodicVehicleInspections = request.PeriodicVehicleInspections,
                PeriodicVehicleInspectionsExp = request.PeriodicVehicleInspectionsExp,
                Weight = request.Weight,
                VehicleCompartment = vehicleCompartments,
                VehicleService = vehicleServices,
                Name = request.Name,
                CompartmentSize = request.VehicleCompartments.Count,
                ProviderVehicle = new List<ProviderVehicle>()
                {
                    new ProviderVehicle() { ProviderId = request.ProviderId }
                }
            };

            var result = await _repository.AddAsync(entity);
            return new Success<Vehicle>() { Data = result };
        }

        private string? _validationError;

        private List<VehicleCompartment>? BuildVehicleCompartments(AddVehicleCommand request)
        {
            var vehicleCompartments = new List<VehicleCompartment>();
            for (int i = 0; i < request.VehicleCompartments.Count; i++)
            {
                var capacity = request.VehicleCompartments[i];
                if (capacity <= 0)
                {
                    _validationError = $"Compartment {i + 1} must have a capacity greater than 0.";
                    return null;
                }

                vehicleCompartments.Add(new VehicleCompartment() { Capacity = capacity });
            }

            return vehicleCompartments;
        }

        private List<VehicleService>? BuildVehicleServices(AddVehicleCommand request)
        {
            var vehicleServices = new List<VehicleService>();
            foreach (var service in request.VehicleService)
            {
                if (service.Capacity <= 0)
                {
                    _validationError = $"The capacity for must be greater than 0.";
                    return null;
                }

                vehicleServices.Add(new VehicleService()
                {
                    VehicleServiceStatus = service.VehicleServiceStatus,
                    Capacity = service.Capacity
                });
            }

            return vehicleServices;
        }

        private static void UpdateVehicle(Vehicle entity, AddVehicleCommand request)
        {
            entity.ProviderId = request.ProviderId;
            entity.Capacity = request.Capacity;
            entity.LicenseNumber = request.LicenseNumber;
            entity.VehicleRegistration = request.VehicleRegistration;
            entity.VehicleRegistrationExp = request.VehicleRegistrationExp;
            entity.VehicleInsurance = request.VehicleInsurance;
            entity.VehicleInsuranceExp = request.VehicleInsuranceExp;
            entity.InspectionReport = request.InspectionReport;
            entity.InspectionReportExp = request.InspectionReportExp;
            entity.Picture = request.Picture;
            entity.MeasurementCertificate = request.MeasurementCertificate;
            entity.PeriodicVehicleInspections = request.PeriodicVehicleInspections;
            entity.PeriodicVehicleInspectionsExp = request.PeriodicVehicleInspectionsExp;
            entity.Weight = request.Weight;
            entity.Name = request.Name;
            entity.CompartmentSize = request.VehicleCompartments.Count;
        }
    }
}
