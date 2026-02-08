using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin.Company;
using CMPNatural.Application.Model;
using CMPNatural.Application.Services;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Helper;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AdminImportCompanyExcelHandler : IRequestHandler<AdminImportCompanyExcelCommand, CommandResponse<CompanyExcelImportResult>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IOperationalAddressRepository _operationalAddressRepository;
        private readonly ILogger<AdminImportCompanyExcelHandler> _logger;
        private readonly IConfiguration _configuration;

        public AdminImportCompanyExcelHandler(ICompanyRepository companyRepository, IOperationalAddressRepository operationalAddressRepository, ILogger<AdminImportCompanyExcelHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _companyRepository = companyRepository;
            _operationalAddressRepository = operationalAddressRepository;
            _configuration = configuration;
        }

        public async Task<CommandResponse<CompanyExcelImportResult>> Handle(AdminImportCompanyExcelCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
                return new NoAcess<CompanyExcelImportResult>() { Message = "Excel file is required." };

            var rows = CompanyExcelImportService.Parse(request.File, request.StartRow, request.WorksheetName);

            var result = new CompanyExcelImportResult
            {
                TotalRows = rows.Count,
                ImportedRows = 0,
                ColumnMap = new CompanyExcelColumnMap()
            };

            var companyCache = new Dictionary<string, Core.Entities.Company>(StringComparer.OrdinalIgnoreCase);
            var operationalAddressCache = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var geocodeCache = new Dictionary<string, GeocodeResult>(StringComparer.OrdinalIgnoreCase);
            var geocodeService = new GoogleGeocodingService(_configuration["GOOGLE_MAPS_API_KEY"] ?? string.Empty);

            foreach (var row in rows)
            {
                var rowResult = new CompanyExcelImportRowResult
                {
                    RowNumber = row.RowNumber,
                    CompanyName = row.CompanyName,
                    OperationalUsername = row.OperationalUsername
                };

                try
                {
                    var missing = new List<string>();
                    var companyName = (row.CompanyName ?? string.Empty).Trim();
                    var operationalUsername = (row.OperationalUsername ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(operationalUsername))
                        missing.Add("OperationalAddress.Username/Password");

                    if (string.IsNullOrWhiteSpace(companyName))
                        missing.Add("CompanyName");

                    var address = BuildAddress(row.Street, row.City, row.State, row.Zip);
                    if (string.IsNullOrWhiteSpace(address))
                        missing.Add("OperationalAddress.Address");

                    var phone = (row.Phone ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(phone))
                        missing.Add("OperationalAddress.Phone");

                    var ContactPerson = (row.ContactPerson ?? string.Empty);
                    if (string.IsNullOrWhiteSpace(ContactPerson))
                        missing.Add("OperationalAddress.ContactPerson");

                    var email = BuildPlaceholderEmail(operationalUsername, row.RowNumber);

                    rowResult.MissingFields = missing;

                    GeocodeResult? geocode = null;
                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        if (!geocodeCache.TryGetValue(address, out var cachedGeo))
                        {
                            geocode = await geocodeService.GeocodeAsync(address, cancellationToken);
                            if (geocode != null)
                            {
                                geocodeCache[address] = geocode;
                            }
                            else
                            {
                                missing.Add("OperationalAddress.Location");
                            }
                        }
                        else
                        {
                            geocode = cachedGeo;
                        }
                    }

                    Core.Entities.Company? companyResult = null;
                    //_logger.LogInformation($"C: {companyName} L: {operationalUsername}");
                    if (!string.IsNullOrWhiteSpace(companyName))
                    {
                        companyCache.TryGetValue(companyName, out companyResult);
                    }

                    if (companyResult == null)
                    {
                        var company = new Core.Entities.Company
                        {
                            CompanyName = string.IsNullOrWhiteSpace(companyName) ? "Unknown" : companyName,
                            PrimaryFirstName = "Unknown",
                            PrimaryLastName = "Unknown",
                            PrimaryPhonNumber = string.IsNullOrWhiteSpace(phone) ? "N/A" : phone,
                            BusinessEmail = email,
                            Position = "Unknown",
                            SecondaryFirstName = null,
                            SecondaryLastName = null,
                            SecondaryPhoneNumber = null,
                            ReferredBy = string.Empty,
                            AccountNumber = string.Empty,
                            Type = (int)CompanyType.Chain,
                            Registered = true,
                            Accepted = true,
                            Status = missing.Count==0 ? CompanyStatus.Approved: CompanyStatus.Incomplete_information,
                            CorporateAddress = address,
                            Password = PasswordGenerator.GenerateSecurePassword(),
                        };

                        companyResult = await _companyRepository.AddAsync(company);
                        if (!string.IsNullOrWhiteSpace(companyName))
                        {
                            companyCache[companyName] = companyResult;
                        }
                    }

                    rowResult.CompanyId = companyResult.Id;

                    if (!string.IsNullOrWhiteSpace(operationalUsername))
                    {
                        var operationalKey = $"{companyResult.Id}:{operationalUsername}".ToLower();
                        if (operationalAddressCache.TryGetValue(operationalKey, out var existingOperationalId))
                        {
                            rowResult.OperationalAddressId = existingOperationalId;
                            result.Rows.Add(rowResult);
                            continue;
                        }

                        var existingOperational = (await _operationalAddressRepository.GetAsync(x =>
                                x.CompanyId == companyResult.Id && x.Username == operationalUsername))
                            .FirstOrDefault();

                        if (existingOperational != null)
                        {
                            rowResult.OperationalAddressId = existingOperational.Id;
                            operationalAddressCache[operationalKey] = existingOperational.Id;
                            result.Rows.Add(rowResult);
                            continue;
                        }
                    }

                    var parts = ContactPerson.Split('#');

                    var part0 = parts.Length > 0 ? parts[0] : string.Empty;
                    var part1 = parts.Length > 1 ? parts[1] : string.Empty;

                    var operationalAddress = new OperationalAddress
                    {
                        CompanyId = companyResult.Id,
                        Name = companyResult.CompanyName,
                        Address = address,
                        LocationPhone = string.IsNullOrWhiteSpace(phone) ? string.Empty : phone,
                        Username = string.IsNullOrWhiteSpace(operationalUsername) ? null : operationalUsername,
                        Password = string.IsNullOrWhiteSpace(operationalUsername) ? null : operationalUsername,
                        FirstName = part0,
                        LastName = part1,
                        Lat = geocode?.Lat,
                        Long = geocode?.Lng,
                        County = geocode?.County
                    };

                    var operationalResult = await _operationalAddressRepository.AddAsync(operationalAddress);
                    rowResult.OperationalAddressId = operationalResult.Id;

                    rowResult.MissingFields = missing;
                    result.ImportedRows += 1;
                    if (!string.IsNullOrWhiteSpace(operationalUsername))
                    {
                        var operationalKey = $"{companyResult.Id}:{operationalUsername}".ToLower();
                        operationalAddressCache[operationalKey] = operationalResult.Id;
                    }
                    _logger.LogInformation(result.ImportedRows.ToString());
                }
                catch (Exception ex)
                {
                    rowResult.Error = ex.Message;
                }

                result.Rows.Add(rowResult);
            }

            return new Success<CompanyExcelImportResult>() { Data = result };
        }

        private static string BuildPlaceholderEmail(string operationalUsername, int rowNumber)
        {
            var safeUsername = string.IsNullOrWhiteSpace(operationalUsername) ? "import" : operationalUsername.Replace(" ", string.Empty);
            return $"imported-{safeUsername}-{rowNumber}@example.invalid";
        }

        private static string BuildAddress(string? street, string? city, string? state, string? zip)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(street)) parts.Add(street.Trim());
            if (!string.IsNullOrWhiteSpace(city)) parts.Add(city.Trim());
            if (!string.IsNullOrWhiteSpace(state)) parts.Add(state.Trim());
            if (!string.IsNullOrWhiteSpace(zip)) parts.Add(zip.Trim());
            return string.Join(", ", parts);
        }
    }
}
