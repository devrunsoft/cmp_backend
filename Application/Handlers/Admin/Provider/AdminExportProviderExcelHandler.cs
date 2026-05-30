using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Model;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminExportProviderExcelHandler : IRequestHandler<AdminExportProviderExcelCommand, ProviderExcelFileResult>
    {
        private readonly IProviderReposiotry _providerReposiotry;
        private readonly IProviderDriverRepository _providerDriverRepository;
        private readonly IProviderVehicleRepository _providerVehicleRepository;

        public AdminExportProviderExcelHandler(
            IProviderReposiotry providerReposiotry,
            IProviderDriverRepository providerDriverRepository,
            IProviderVehicleRepository providerVehicleRepository)
        {
            _providerReposiotry = providerReposiotry;
            _providerDriverRepository = providerDriverRepository;
            _providerVehicleRepository = providerVehicleRepository;
        }

        public async Task<ProviderExcelFileResult> Handle(AdminExportProviderExcelCommand request, CancellationToken cancellationToken)
        {
            var providers = (await _providerReposiotry.GetAsync(
                x => true,
                query => query.Include(x => x.ProviderService)))
                .OrderBy(x => x.Id)
                .ToList();

            var providerDrivers = (await _providerDriverRepository.GetAsync(
                x => true,
                query => query
                    .Include(x => x.Provider)
                    .Include(x => x.Driver)
                    .ThenInclude(x => x.Person)))
                .OrderBy(x => x.ProviderId)
                .ThenBy(x => x.DriverId)
                .ToList();

            var providerVehicles = (await _providerVehicleRepository.GetAsync(
                x => true,
                query => query
                    .Include(x => x.Provider)
                    .Include(x => x.Vehicle)
                    .ThenInclude(x => x.VehicleCompartment)
                    .Include(x => x.Vehicle)
                    .ThenInclude(x => x.VehicleService)))
                .OrderBy(x => x.ProviderId)
                .ThenBy(x => x.VehicleId)
                .ToList();

            var payload = new ProviderDatabaseExportPayload
            {
                ExportedAtUtc = System.DateTime.UtcNow,
                Providers = providers.Select(x => new ProviderDbRow
                {
                    Id = x.Id,
                    PersonId = x.PersonId,
                    Name = x.Name,
                    Rating = x.Rating,
                    Status = (int)x.Status,
                    Lat = x.Lat,
                    Long = x.Long,
                    AreaLocation = x.AreaLocation,
                    City = x.City,
                    Address = x.Address,
                    County = x.County,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Password = x.Password,
                    RegistrationStatus = (int)x.RegistrationStatus,
                    BusinessLicense = x.BusinessLicense,
                    BusinessLicenseExp = x.BusinessLicenseExp,
                    HealthDepartmentPermit = x.HealthDepartmentPermit,
                    HealthDepartmentPermitExp = x.HealthDepartmentPermitExp,
                    WasteHaulerPermit = x.WasteHaulerPermit,
                    EPACompliance = x.EPACompliance,
                    EPAComplianceExp = x.EPAComplianceExp,
                    Insurance = x.Insurance,
                    InsuranceExp = x.InsuranceExp,
                    ActivationLink = x.ActivationLink,
                    HasLogin = x.HasLogin,
                    ManagerFirstName = x.ManagerFirstName,
                    ManagerLastName = x.ManagerLastName,
                    ManagerPhoneNumber = x.ManagerPhoneNumber
                }).ToList(),
                ProviderServices = providers
                    .SelectMany(x => x.ProviderService)
                    .Select(x => new ProviderServiceDbRow
                    {
                        Id = x.Id,
                        ProviderId = x.ProviderId,
                        ProductId = x.ProductId
                    })
                    .OrderBy(x => x.Id)
                    .ToList(),
                Persons = providers
                    .Where(x => x.PersonId.HasValue)
                    .Select(x => x.PersonId!.Value)
                    .Concat(providerDrivers.Where(x => x.Driver != null).Select(x => x.Driver.PersonId))
                    .Distinct()
                    .Select(personId =>
                    {
                        var person = providerDrivers.Select(x => x.Driver?.Person).FirstOrDefault(x => x != null && x.Id == personId);
                        return person == null
                            ? null
                            : new PersonDbRow
                            {
                                Id = person.Id,
                                FirstName = person.FirstName,
                                LastName = person.LastName
                            };
                    })
                    .Where(x => x != null)
                    .Cast<PersonDbRow>()
                    .ToList(),
                Drivers = providerDrivers
                    .Where(x => x.Driver != null)
                    .Select(x => x.Driver!)
                    .GroupBy(x => x.Id)
                    .Select(x => x.First())
                    .OrderBy(x => x.Id)
                    .Select(x => new DriverDbRow
                    {
                        Id = x.Id,
                        PersonId = x.PersonId,
                        Email = x.Email,
                        Password = x.Password,
                        License = x.License,
                        LicenseExp = x.LicenseExp,
                        BackgroundCheck = x.BackgroundCheck,
                        BackgroundCheckExp = x.BackgroundCheckExp,
                        ProfilePhoto = x.ProfilePhoto,
                        ActivationLink = x.ActivationLink,
                        Status = (int)x.Status,
                        TwoFactor = x.TwoFactor
                    })
                    .ToList(),
                ProviderDrivers = providerDrivers.Select(x => new ProviderDriverDbRow
                {
                    Id = x.Id,
                    ProviderId = x.ProviderId,
                    DriverId = x.DriverId,
                    IsDefault = x.IsDefault
                }).ToList(),
                Vehicles = providerVehicles
                    .Where(x => x.Vehicle != null)
                    .Select(x => x.Vehicle!)
                    .GroupBy(x => x.Id)
                    .Select(x => x.First())
                    .OrderBy(x => x.Id)
                    .Select(x => new VehicleDbRow
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProviderId = x.ProviderId,
                        VehicleRegistration = x.VehicleRegistration,
                        VehicleRegistrationExp = x.VehicleRegistrationExp,
                        VehicleInsurance = x.VehicleInsurance,
                        VehicleInsuranceExp = x.VehicleInsuranceExp,
                        InspectionReport = x.InspectionReport,
                        InspectionReportExp = x.InspectionReportExp,
                        Picture = x.Picture,
                        Capacity = x.Capacity,
                        Weight = x.Weight,
                        MeasurementCertificate = x.MeasurementCertificate,
                        PeriodicVehicleInspections = x.PeriodicVehicleInspections,
                        PeriodicVehicleInspectionsExp = x.PeriodicVehicleInspectionsExp,
                        CompartmentSize = x.CompartmentSize,
                        LicenseNumber = x.LicenseNumber
                    })
                    .ToList(),
                ProviderVehicles = providerVehicles.Select(x => new ProviderVehicleDbRow
                {
                    Id = x.Id,
                    ProviderId = x.ProviderId,
                    VehicleId = x.VehicleId
                }).ToList(),
                VehicleCompartments = providerVehicles
                    .Where(x => x.Vehicle != null)
                    .SelectMany(x => x.Vehicle!.VehicleCompartment)
                    .GroupBy(x => x.Id)
                    .Select(x => x.First())
                    .OrderBy(x => x.Id)
                    .Select(x => new VehicleCompartmentDbRow
                    {
                        Id = x.Id,
                        VehicleId = x.VehicleId,
                        Capacity = x.Capacity
                    })
                    .ToList(),
                VehicleServices = providerVehicles
                    .Where(x => x.Vehicle != null)
                    .SelectMany(x => x.Vehicle!.VehicleService)
                    .GroupBy(x => x.Id)
                    .Select(x => x.First())
                    .OrderBy(x => x.Id)
                    .Select(x => new VehicleServiceDbRow
                    {
                        Id = x.Id,
                        VehicleId = x.VehicleId,
                        Capacity = x.Capacity,
                        VehicleServiceStatus = (int)x.VehicleServiceStatus
                    })
                    .ToList()
            };

            var content = JsonSerializer.SerializeToUtf8Bytes(payload, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            return new ProviderExcelFileResult
            {
                FileName = $"providers-db-{System.DateTime.UtcNow:yyyyMMddHHmmss}.json",
                ContentType = "application/json",
                Content = content
            };
        }
    }
}
