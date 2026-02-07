using System;
using System.Collections.Generic;
using System.Globalization;
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

        public AdminImportCompanyExcelHandler(ICompanyRepository companyRepository, IOperationalAddressRepository operationalAddressRepository, ILogger<AdminImportCompanyExcelHandler> logger)
        {
            _logger = logger;
            _companyRepository = companyRepository;
            _operationalAddressRepository = operationalAddressRepository;
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
                    if (string.IsNullOrWhiteSpace(companyName))
                        missing.Add("CompanyName");

                    var operationalUsername = (row.OperationalUsername ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(operationalUsername))
                        missing.Add("OperationalAddress.Username/Password");

                    var address = BuildAddress(row.Street, row.City, row.State, row.Zip);
                    if (string.IsNullOrWhiteSpace(address))
                        missing.Add("OperationalAddress.Address");

                    var phone = (row.Phone ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(phone))
                        missing.Add("OperationalAddress.Phone");

                    var email = BuildPlaceholderEmail(operationalUsername, row.RowNumber);
                    missing.Add("Company.BusinessEmail");

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
                        Registered = missing.Count == 0,
                        Accepted = true,
                        Status = CompanyStatus.Approved,
                        CorporateAddress = address,
                        Password = PasswordGenerator.GenerateSecurePassword()
                    };

                    var companyResult = await _companyRepository.AddAsync(company);
                    rowResult.CompanyId = companyResult.Id;

                    var operationalAddress = new OperationalAddress
                    {
                        CompanyId = companyResult.Id,
                        Name = companyResult.CompanyName,
                        Address = address,
                        LocationPhone = string.IsNullOrWhiteSpace(phone) ? string.Empty : phone,
                        Username = string.IsNullOrWhiteSpace(operationalUsername) ? null : operationalUsername,
                        Password = string.IsNullOrWhiteSpace(operationalUsername) ? null : operationalUsername,
                        FirstName = string.Empty,
                        LastName = string.Empty
                    };

                    var operationalResult = await _operationalAddressRepository.AddAsync(operationalAddress);
                    rowResult.OperationalAddressId = operationalResult.Id;

                    rowResult.MissingFields = missing;
                    result.ImportedRows += 1;
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
