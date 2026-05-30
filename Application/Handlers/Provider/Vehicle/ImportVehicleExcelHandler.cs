using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Model;
using CMPNatural.Application.Services;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ImportVehicleExcelHandler : IRequestHandler<ImportVehicleExcelCommand, CommandResponse<VehicleExcelImportResult>>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IProviderVehicleRepository _providerVehicleRepository;
        private readonly IVehicleCompartmentRepository _vehicleCompartmentRepository;
        private readonly IVehicleServiceRepository _vehicleServiceRepository;

        public ImportVehicleExcelHandler(
            IVehicleRepository vehicleRepository,
            IProviderVehicleRepository providerVehicleRepository,
            IVehicleCompartmentRepository vehicleCompartmentRepository,
            IVehicleServiceRepository vehicleServiceRepository)
        {
            _vehicleRepository = vehicleRepository;
            _providerVehicleRepository = providerVehicleRepository;
            _vehicleCompartmentRepository = vehicleCompartmentRepository;
            _vehicleServiceRepository = vehicleServiceRepository;
        }

        public async Task<CommandResponse<VehicleExcelImportResult>> Handle(ImportVehicleExcelCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
                return new NoAcess<VehicleExcelImportResult> { Message = "Excel file is required." };

            var rows = VehicleExcelService.Parse(request.File, request.StartRow, request.WorksheetName);
            var result = new VehicleExcelImportResult
            {
                TotalRows = rows.Count
            };

            var vehicles = (await _vehicleRepository.GetAsync(
                x => true,
                query => query
                    .Include(x => x.ProviderVehicle)
                    .Include(x => x.VehicleCompartment)
                    .Include(x => x.VehicleService)))
                .ToList();

            var vehiclesByLicense = vehicles
                .Where(x => !string.IsNullOrWhiteSpace(x.LicenseNumber))
                .GroupBy(x => NormalizeLicense(x.LicenseNumber))
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var rowResult = new VehicleExcelImportRowResult
                {
                    RowNumber = row.RowNumber,
                    LicenseNumber = row.LicenseNumber
                };

                try
                {
                    var missingFields = new List<string>();
                    if (string.IsNullOrWhiteSpace(row.Name))
                        missingFields.Add("Name");
                    var license = NormalizeLicense(row.LicenseNumber);
                    if (string.IsNullOrWhiteSpace(license))
                        missingFields.Add("LicenseNumber");
                    if (!row.Capacity.HasValue)
                        missingFields.Add("Capacity");

                    rowResult.MissingFields = missingFields;
                    if (missingFields.Count > 0)
                    {
                        result.Rows.Add(rowResult);
                        continue;
                    }

                    var compartmentValues = ParseCompartments(row.VehicleCompartments);
                    var serviceValues = ParseVehicleServices(row.VehicleServices);
                    if (compartmentValues.Any(x => x <= 0))
                        throw new InvalidOperationException("VehicleCompartments must contain capacities greater than 0.");
                    if (serviceValues.Any(x => x.Capacity <= 0))
                        throw new InvalidOperationException("VehicleServices must contain capacities greater than 0.");

                    vehiclesByLicense.TryGetValue(license, out var vehicle);
                    var isCreate = vehicle == null;

                    if (vehicle == null)
                    {
                        vehicle = new Vehicle
                        {
                            LicenseNumber = license,
                            ProviderVehicle = new List<ProviderVehicle>(),
                            VehicleCompartment = new List<VehicleCompartment>(),
                            VehicleService = new List<VehicleService>()
                        };
                        vehiclesByLicense[license] = vehicle;
                    }

                    ApplyRow(vehicle, row, request.ProviderId, license);

                    if (isCreate)
                    {
                        vehicle.VehicleCompartment = compartmentValues.Select(x => new VehicleCompartment { Capacity = x }).ToList();
                        vehicle.VehicleService = serviceValues.Select(x => new VehicleService
                        {
                            VehicleServiceStatus = x.Status,
                            Capacity = x.Capacity
                        }).ToList();
                        vehicle.ProviderVehicle.Add(new ProviderVehicle { ProviderId = request.ProviderId });

                        vehicle = await _vehicleRepository.AddAsync(vehicle);
                        vehiclesByLicense[license] = vehicle;
                        result.CreatedRows += 1;
                        rowResult.Action = "Created";
                    }
                    else
                    {
                        var hasRelation = vehicle.ProviderVehicle.Any(x => x.ProviderId == request.ProviderId && x.VehicleId == vehicle.Id);
                        if (!hasRelation)
                            await _providerVehicleRepository.AddAsync(new ProviderVehicle { ProviderId = request.ProviderId, VehicleId = vehicle.Id });

                        var existingCompartments = vehicle.VehicleCompartment.ToList();
                        if (existingCompartments.Count > 0)
                            await _vehicleCompartmentRepository.DeleteRangeAsync(existingCompartments);

                        var existingServices = vehicle.VehicleService.ToList();
                        if (existingServices.Count > 0)
                            await _vehicleServiceRepository.DeleteRangeAsync(existingServices);

                        var newCompartments = compartmentValues.Select(x => new VehicleCompartment { VehicleId = vehicle.Id, Capacity = x }).ToList();
                        if (newCompartments.Count > 0)
                            await _vehicleCompartmentRepository.AddRangeAsync(newCompartments);

                        var newServices = serviceValues.Select(x => new VehicleService
                        {
                            VehicleId = vehicle.Id,
                            VehicleServiceStatus = x.Status,
                            Capacity = x.Capacity
                        }).ToList();
                        if (newServices.Count > 0)
                            await _vehicleServiceRepository.AddRangeAsync(newServices);

                        await _vehicleRepository.UpdateAsync(vehicle);
                        result.UpdatedRows += 1;
                        rowResult.Action = "Updated";
                    }

                    rowResult.VehicleId = vehicle.Id;
                    result.ImportedRows += 1;
                }
                catch (Exception ex)
                {
                    rowResult.Error = ex.Message;
                }

                result.Rows.Add(rowResult);
            }

            return new Success<VehicleExcelImportResult> { Data = result };
        }

        private static void ApplyRow(Vehicle vehicle, VehicleExcelRowData row, long providerId, string license)
        {
            vehicle.Name = row.Name!.Trim();
            vehicle.ProviderId = providerId;
            vehicle.LicenseNumber = license;
            vehicle.Capacity = row.Capacity ?? vehicle.Capacity;
            vehicle.Weight = row.Weight ?? vehicle.Weight;
            vehicle.VehicleRegistration = string.IsNullOrWhiteSpace(row.VehicleRegistration) ? vehicle.VehicleRegistration : row.VehicleRegistration.Trim();
            vehicle.VehicleRegistrationExp = row.VehicleRegistrationExp ?? vehicle.VehicleRegistrationExp;
            vehicle.VehicleInsurance = string.IsNullOrWhiteSpace(row.VehicleInsurance) ? vehicle.VehicleInsurance : row.VehicleInsurance.Trim();
            vehicle.VehicleInsuranceExp = row.VehicleInsuranceExp ?? vehicle.VehicleInsuranceExp;
            vehicle.InspectionReport = string.IsNullOrWhiteSpace(row.InspectionReport) ? vehicle.InspectionReport : row.InspectionReport.Trim();
            vehicle.InspectionReportExp = row.InspectionReportExp ?? vehicle.InspectionReportExp;
            vehicle.Picture = string.IsNullOrWhiteSpace(row.Picture) ? vehicle.Picture : row.Picture.Trim();
            vehicle.MeasurementCertificate = string.IsNullOrWhiteSpace(row.MeasurementCertificate) ? vehicle.MeasurementCertificate : row.MeasurementCertificate.Trim();
            vehicle.PeriodicVehicleInspections = string.IsNullOrWhiteSpace(row.PeriodicVehicleInspections) ? vehicle.PeriodicVehicleInspections : row.PeriodicVehicleInspections.Trim();
            vehicle.PeriodicVehicleInspectionsExp = row.PeriodicVehicleInspectionsExp ?? vehicle.PeriodicVehicleInspectionsExp;
            vehicle.CompartmentSize = CountValues(row.VehicleCompartments);
        }

        private static List<int> ParseCompartments(string? value)
        {
            return SplitValues(value)
                .Select(x => int.Parse(x, CultureInfo.InvariantCulture))
                .ToList();
        }

        private static List<(VehicleServiceStatus Status, int Capacity)> ParseVehicleServices(string? value)
        {
            var services = new List<(VehicleServiceStatus Status, int Capacity)>();
            foreach (var token in SplitValues(value))
            {
                var pieces = token.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (pieces.Length != 2)
                    throw new InvalidOperationException("VehicleServices format must be ServiceStatus:Capacity.");

                if (!Enum.TryParse<VehicleServiceStatus>(pieces[0].Trim(), true, out var status))
                    throw new InvalidOperationException($"Vehicle service status '{pieces[0].Trim()}' is invalid.");

                if (!int.TryParse(pieces[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var capacity))
                    throw new InvalidOperationException($"Vehicle service capacity '{pieces[1].Trim()}' is invalid.");

                services.Add((status, capacity));
            }

            return services;
        }

        private static int CountValues(string? value)
        {
            return SplitValues(value).Count();
        }

        private static IEnumerable<string> SplitValues(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Enumerable.Empty<string>();

            return value
                .Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private static string NormalizeLicense(string? licenseNumber)
        {
            return (licenseNumber ?? string.Empty).Trim();
        }
    }
}
