using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Model;
using CMPNatural.Application.Services;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Helper;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminImportProviderExcelHandler : IRequestHandler<AdminImportProviderExcelCommand, CommandResponse<ProviderExcelImportResult>>
    {
        private static readonly Regex EmailPattern = new Regex(@"^[\w\.\+-]+@([\w-]+\.)+[a-zA-Z]{2,7}$", RegexOptions.Compiled);

        private readonly IProviderReposiotry _providerReposiotry;
        private readonly IProviderServiceRepository _providerServiceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IProviderDriverRepository _providerDriverRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IProviderVehicleRepository _providerVehicleRepository;
        private readonly IVehicleCompartmentRepository _vehicleCompartmentRepository;
        private readonly IVehicleServiceRepository _vehicleServiceRepository;

        public AdminImportProviderExcelHandler(
            IProviderReposiotry providerReposiotry,
            IProviderServiceRepository providerServiceRepository,
            IProductRepository productRepository,
            IPersonRepository personRepository,
            IDriverRepository driverRepository,
            IProviderDriverRepository providerDriverRepository,
            IVehicleRepository vehicleRepository,
            IProviderVehicleRepository providerVehicleRepository,
            IVehicleCompartmentRepository vehicleCompartmentRepository,
            IVehicleServiceRepository vehicleServiceRepository)
        {
            _providerReposiotry = providerReposiotry;
            _providerServiceRepository = providerServiceRepository;
            _productRepository = productRepository;
            _personRepository = personRepository;
            _driverRepository = driverRepository;
            _providerDriverRepository = providerDriverRepository;
            _vehicleRepository = vehicleRepository;
            _providerVehicleRepository = providerVehicleRepository;
            _vehicleCompartmentRepository = vehicleCompartmentRepository;
            _vehicleServiceRepository = vehicleServiceRepository;
        }

        public async Task<CommandResponse<ProviderExcelImportResult>> Handle(AdminImportProviderExcelCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
                return new NoAcess<ProviderExcelImportResult>() { Message = "Excel file is required." };

            var workbookData = ProviderExcelService.ParseWorkbook(request.File, request.StartRow);
            var rows = workbookData.Providers;
            var result = new ProviderExcelImportResult
            {
                TotalRows = rows.Count
            };

            var providers = (await _providerReposiotry.GetAsync(
                x => true,
                query => query.Include(x => x.ProviderService)))
                .ToList();
            var providersByEmail = providers
                .Where(x => !string.IsNullOrWhiteSpace(x.Email))
                .GroupBy(x => NormalizeEmail(x.Email))
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

            var products = (await _productRepository.GetAllAsync()).ToList();
            var productsById = products.ToDictionary(x => x.Id, x => x);
            var productsByName = products
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .GroupBy(x => x.Name.Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var rowResult = new ProviderExcelImportRowResult
                {
                    RowNumber = row.RowNumber,
                    Name = row.Name,
                    Email = row.Email
                };

                try
                {
                    var missingFields = new List<string>();
                    var email = NormalizeEmail(row.Email);
                    if (string.IsNullOrWhiteSpace(email))
                        missingFields.Add("Email");

                    var name = row.Name?.Trim();
                    if (string.IsNullOrWhiteSpace(name))
                        missingFields.Add("Name");

                    rowResult.MissingFields = missingFields;
                    if (missingFields.Count > 0)
                    {
                        result.Rows.Add(rowResult);
                        continue;
                    }

                    if (!EmailPattern.IsMatch(email))
                    {
                        rowResult.Error = "The email format is invalid.";
                        result.Rows.Add(rowResult);
                        continue;
                    }

                    var providerStatus = ParseProviderStatus(row.Status);
                    var productIds = ResolveProductIds(row, productsById, productsByName);

                    providersByEmail.TryGetValue(email, out var provider);
                    var isCreate = provider == null;

                    if (provider == null)
                    {
                        provider = new Provider
                        {
                            Email = email,
                            Password = string.IsNullOrWhiteSpace(row.Password) ? PasswordGenerator.GenerateSecurePassword() : row.Password.Trim()
                        };

                        await EnsureProviderPersonAsync(provider, row, cancellationToken);
                    }
                    else
                    {
                        provider.Password = string.IsNullOrWhiteSpace(row.Password) ? provider.Password : row.Password.Trim();
                        await EnsureProviderPersonAsync(provider, row, cancellationToken);
                    }

                    provider.Name = name!;
                    provider.PhoneNumber = row.PhoneNumber?.Trim() ?? string.Empty;
                    provider.Status = providerStatus ?? provider.Status;
                    provider.City = row.City?.Trim();
                    provider.Address = row.Address?.Trim();
                    provider.County = row.County?.Trim();
                    provider.Lat = row.Lat ?? provider.Lat ?? 0;
                    provider.Long = row.Long ?? provider.Long ?? 0;
                    provider.AreaLocation = row.AreaLocation ?? provider.AreaLocation ?? 0;
                    provider.Rating = row.Rating ?? provider.Rating ?? 0;
                    provider.ManagerFirstName = row.ManagerFirstName?.Trim();
                    provider.ManagerLastName = row.ManagerLastName?.Trim();
                    provider.ManagerPhoneNumber = row.ManagerPhoneNumber?.Trim();

                    if (isCreate)
                    {
                        provider = await _providerReposiotry.AddAsync(provider);
                        providersByEmail[email] = provider;
                        result.CreatedRows += 1;
                        rowResult.Action = "Created";
                    }
                    else
                    {
                        await _providerReposiotry.UpdateAsync(provider);
                        result.UpdatedRows += 1;
                        rowResult.Action = "Updated";
                    }

                    var existingServices = (await _providerServiceRepository.GetAsync(x => x.ProviderId == provider.Id)).ToList();
                    if (existingServices.Count > 0)
                        await _providerServiceRepository.DeleteRangeAsync(existingServices);

                    if (productIds.Count > 0)
                    {
                        var providerServices = productIds
                            .Select(productId => new ProviderService { ProviderId = provider.Id, ProductId = productId })
                            .ToList();
                        await _providerServiceRepository.AddRangeAsync(providerServices);
                    }

                    rowResult.ProviderId = provider.Id;
                    result.ImportedRows += 1;
                }
                catch (Exception ex)
                {
                    rowResult.Error = ex.Message;
                }

                result.Rows.Add(rowResult);
            }

            await ImportDriversAsync(workbookData.Drivers, providersByEmail, result.Drivers);
            await ImportVehiclesAsync(workbookData.Vehicles, providersByEmail, result.Vehicles);

            return new Success<ProviderExcelImportResult>() { Data = result };
        }

        private async Task EnsureProviderPersonAsync(Provider provider, ProviderExcelRowData row, CancellationToken cancellationToken)
        {
            if (provider.PersonId == null)
            {
                var personId = Guid.NewGuid();
                var person = new Person
                {
                    Id = personId,
                    FirstName = row.ManagerFirstName ?? string.Empty,
                    LastName = row.ManagerLastName ?? string.Empty
                };
                await _personRepository.AddAsync(person);
                provider.PersonId = personId;
                return;
            }

            var existingPerson = await _personRepository.GetByIdAsync(provider.PersonId.Value);
            if (existingPerson == null)
                return;

            existingPerson.FirstName = row.ManagerFirstName ?? existingPerson.FirstName;
            existingPerson.LastName = row.ManagerLastName ?? existingPerson.LastName;
            await _personRepository.UpdateAsync(existingPerson);
        }

        private static string NormalizeEmail(string? email)
        {
            return (email ?? string.Empty).Trim().ToLowerInvariant();
        }

        private static ProviderStatus? ParseProviderStatus(string? rawStatus)
        {
            if (string.IsNullOrWhiteSpace(rawStatus))
                return null;

            if (Enum.TryParse<ProviderStatus>(rawStatus.Trim(), true, out var enumStatus))
                return enumStatus;

            if (int.TryParse(rawStatus.Trim(), out var statusValue) && Enum.IsDefined(typeof(ProviderStatus), statusValue))
                return (ProviderStatus)statusValue;

            return null;
        }

        private static List<long> ResolveProductIds(
            ProviderExcelRowData row,
            IReadOnlyDictionary<long, Product> productsById,
            IReadOnlyDictionary<string, Product> productsByName)
        {
            var resolved = new HashSet<long>();
            foreach (var token in SplitValues(row.ProductIds))
            {
                if (long.TryParse(token, out var productId) && productsById.ContainsKey(productId))
                    resolved.Add(productId);
            }

            foreach (var token in SplitValues(row.ProductNames))
            {
                if (productsByName.TryGetValue(token, out var product))
                    resolved.Add(product.Id);
            }

            return resolved.ToList();
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

        private async Task ImportDriversAsync(
            IReadOnlyList<ProviderDriverSheetRowData> rows,
            IDictionary<string, Provider> providersByEmail,
            DriverExcelImportResult result)
        {
            result.TotalRows = rows.Count;
            var providerDrivers = (await _providerDriverRepository.GetAsync(
                x => true,
                query => query.Include(x => x.Driver).ThenInclude(x => x.Person)))
                .ToList();

            var scopedDrivers = providerDrivers
                .Where(x => x.Driver != null && !string.IsNullOrWhiteSpace(x.Driver.Email))
                .GroupBy(x => $"{x.ProviderId}:{NormalizeEmail(x.Driver!.Email)}", StringComparer.OrdinalIgnoreCase)
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

            var allDriversByEmail = (await _driverRepository.GetAsync(
                x => true,
                query => query.Include(x => x.Person).Include(x => x.ProviderDriver)))
                .Where(x => !string.IsNullOrWhiteSpace(x.Email))
                .GroupBy(x => NormalizeEmail(x.Email))
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var rowResult = new DriverExcelImportRowResult
                {
                    RowNumber = row.RowNumber,
                    Email = row.Email
                };

                try
                {
                    var missingFields = new List<string>();
                    var providerEmail = NormalizeEmail(row.ProviderEmail);
                    if (string.IsNullOrWhiteSpace(providerEmail))
                        missingFields.Add("ProviderEmail");
                    if (string.IsNullOrWhiteSpace(row.Email))
                        missingFields.Add("Email");
                    if (string.IsNullOrWhiteSpace(row.FirstName))
                        missingFields.Add("FirstName");
                    if (string.IsNullOrWhiteSpace(row.LastName))
                        missingFields.Add("LastName");

                    rowResult.MissingFields = missingFields;
                    if (missingFields.Count > 0)
                    {
                        result.Rows.Add(rowResult);
                        continue;
                    }

                    if (!providersByEmail.TryGetValue(providerEmail, out var provider))
                    {
                        rowResult.Error = "ProviderEmail was not found in the Providers sheet or database.";
                        result.Rows.Add(rowResult);
                        continue;
                    }

                    var email = NormalizeEmail(row.Email);
                    if (!EmailPattern.IsMatch(email))
                    {
                        rowResult.Error = "The email format is invalid.";
                        result.Rows.Add(rowResult);
                        continue;
                    }

                    var scopedKey = $"{provider.Id}:{email}";
                    scopedDrivers.TryGetValue(scopedKey, out var providerDriver);

                    if (!allDriversByEmail.TryGetValue(email, out var driver))
                    {
                        var person = new Person
                        {
                            Id = Guid.NewGuid(),
                            FirstName = row.FirstName!.Trim(),
                            LastName = row.LastName!.Trim()
                        };

                        driver = new Driver
                        {
                            PersonId = person.Id,
                            Person = person,
                            Email = email,
                            Password = string.IsNullOrWhiteSpace(row.Password) ? PasswordGenerator.GenerateSecurePassword() : row.Password.Trim(),
                            ProviderDriver = new List<ProviderDriver>()
                        };

                        ApplyDriverRow(driver, row);
                        driver = await _driverRepository.AddAsync(driver);
                        allDriversByEmail[email] = driver;
                    }
                    else
                    {
                        driver.Email = email;
                        driver.Password = string.IsNullOrWhiteSpace(row.Password) ? driver.Password : row.Password.Trim();
                        ApplyDriverRow(driver, row);
                        await EnsureDriverPersonAsync(driver, row);
                        await _driverRepository.UpdateAsync(driver);
                    }

                    if (providerDriver == null)
                    {
                        providerDriver = new ProviderDriver
                        {
                            ProviderId = provider.Id,
                            DriverId = driver.Id,
                            IsDefault = row.IsDefault ?? false
                        };
                        await _providerDriverRepository.AddAsync(providerDriver);
                        scopedDrivers[scopedKey] = providerDriver;
                        result.CreatedRows += 1;
                        rowResult.Action = "Created";
                    }
                    else
                    {
                        providerDriver.IsDefault = row.IsDefault ?? providerDriver.IsDefault;
                        await _providerDriverRepository.UpdateAsync(providerDriver);
                        result.UpdatedRows += 1;
                        rowResult.Action = "Updated";
                    }

                    await ResetOtherProviderDefaultsAsync(provider.Id, driver.Id, providerDriver.IsDefault);
                    rowResult.DriverId = driver.Id;
                    result.ImportedRows += 1;
                }
                catch (Exception ex)
                {
                    rowResult.Error = ex.Message;
                }

                result.Rows.Add(rowResult);
            }
        }

        private async Task ImportVehiclesAsync(
            IReadOnlyList<ProviderVehicleSheetRowData> rows,
            IDictionary<string, Provider> providersByEmail,
            VehicleExcelImportResult result)
        {
            result.TotalRows = rows.Count;
            var vehicles = (await _vehicleRepository.GetAsync(
                x => true,
                query => query.Include(x => x.ProviderVehicle).Include(x => x.VehicleCompartment).Include(x => x.VehicleService)))
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
                    var providerEmail = NormalizeEmail(row.ProviderEmail);
                    if (string.IsNullOrWhiteSpace(providerEmail))
                        missingFields.Add("ProviderEmail");
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

                    if (!providersByEmail.TryGetValue(providerEmail, out var provider))
                    {
                        rowResult.Error = "ProviderEmail was not found in the Providers sheet or database.";
                        result.Rows.Add(rowResult);
                        continue;
                    }

                    var compartments = ParseCompartments(row.VehicleCompartments);
                    var services = ParseVehicleServices(row.VehicleServices);
                    if (compartments.Any(x => x <= 0))
                        throw new InvalidOperationException("VehicleCompartments must contain capacities greater than 0.");
                    if (services.Any(x => x.Capacity <= 0))
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

                    ApplyVehicleRow(vehicle, row, provider.Id, license);

                    if (isCreate)
                    {
                        vehicle.VehicleCompartment = compartments.Select(x => new VehicleCompartment { Capacity = x }).ToList();
                        vehicle.VehicleService = services.Select(x => new VehicleService
                        {
                            VehicleServiceStatus = x.Status,
                            Capacity = x.Capacity
                        }).ToList();
                        vehicle.ProviderVehicle.Add(new ProviderVehicle { ProviderId = provider.Id });
                        vehicle = await _vehicleRepository.AddAsync(vehicle);
                        vehiclesByLicense[license] = vehicle;
                        result.CreatedRows += 1;
                        rowResult.Action = "Created";
                    }
                    else
                    {
                        var hasRelation = vehicle.ProviderVehicle.Any(x => x.ProviderId == provider.Id && x.VehicleId == vehicle.Id);
                        if (!hasRelation)
                            await _providerVehicleRepository.AddAsync(new ProviderVehicle { ProviderId = provider.Id, VehicleId = vehicle.Id });

                        var existingCompartments = vehicle.VehicleCompartment.ToList();
                        if (existingCompartments.Count > 0)
                            await _vehicleCompartmentRepository.DeleteRangeAsync(existingCompartments);

                        var existingServices = vehicle.VehicleService.ToList();
                        if (existingServices.Count > 0)
                            await _vehicleServiceRepository.DeleteRangeAsync(existingServices);

                        var newCompartments = compartments.Select(x => new VehicleCompartment { VehicleId = vehicle.Id, Capacity = x }).ToList();
                        if (newCompartments.Count > 0)
                            await _vehicleCompartmentRepository.AddRangeAsync(newCompartments);

                        var newServices = services.Select(x => new VehicleService
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
        }

        private async Task EnsureDriverPersonAsync(Driver driver, ProviderDriverSheetRowData row)
        {
            var person = driver.Person ?? await _personRepository.GetByIdAsync(driver.PersonId);
            if (person == null)
                return;

            person.FirstName = row.FirstName?.Trim() ?? person.FirstName;
            person.LastName = row.LastName?.Trim() ?? person.LastName;
            await _personRepository.UpdateAsync(person);
        }

        private static void ApplyDriverRow(Driver driver, ProviderDriverSheetRowData row)
        {
            driver.License = string.IsNullOrWhiteSpace(row.License) ? driver.License : row.License.Trim();
            driver.LicenseExp = row.LicenseExp ?? driver.LicenseExp;
            driver.BackgroundCheck = string.IsNullOrWhiteSpace(row.BackgroundCheck) ? driver.BackgroundCheck : row.BackgroundCheck.Trim();
            driver.BackgroundCheckExp = row.BackgroundCheckExp ?? driver.BackgroundCheckExp;
            driver.ProfilePhoto = string.IsNullOrWhiteSpace(row.ProfilePhoto) ? driver.ProfilePhoto : row.ProfilePhoto.Trim();
            driver.Status = ParseDriverStatus(row.Status) ?? driver.Status;
        }

        private static void ApplyVehicleRow(Vehicle vehicle, ProviderVehicleSheetRowData row, long providerId, string license)
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
            vehicle.CompartmentSize = SplitValues(row.VehicleCompartments).Count();
        }

        private async Task ResetOtherProviderDefaultsAsync(long providerId, long driverId, bool shouldReset)
        {
            if (!shouldReset)
                return;

            var otherRelations = await _providerDriverRepository.GetAsync(x => x.ProviderId == providerId && x.DriverId != driverId && x.IsDefault);
            foreach (var relation in otherRelations)
            {
                relation.IsDefault = false;
                await _providerDriverRepository.UpdateAsync(relation);
            }
        }

        private static DriverStatus? ParseDriverStatus(string? rawStatus)
        {
            if (string.IsNullOrWhiteSpace(rawStatus))
                return null;

            if (Enum.TryParse<DriverStatus>(rawStatus.Trim(), true, out var enumStatus))
                return enumStatus;

            if (int.TryParse(rawStatus.Trim(), out var statusValue) && Enum.IsDefined(typeof(DriverStatus), statusValue))
                return (DriverStatus)statusValue;

            return null;
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

        private static string NormalizeLicense(string? licenseNumber)
        {
            return (licenseNumber ?? string.Empty).Trim();
        }
    }
}
