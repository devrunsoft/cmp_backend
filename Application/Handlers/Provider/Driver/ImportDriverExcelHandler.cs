using System;
using System.Collections.Generic;
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
    public class ImportDriverExcelHandler : IRequestHandler<ImportDriverExcelCommand, CommandResponse<DriverExcelImportResult>>
    {
        private static readonly Regex EmailPattern = new Regex(@"^[\w\.\+-]+@([\w-]+\.)+[a-zA-Z]{2,7}$", RegexOptions.Compiled);

        private readonly IDriverRepository _driverRepository;
        private readonly IProviderDriverRepository _providerDriverRepository;
        private readonly IPersonRepository _personRepository;

        public ImportDriverExcelHandler(
            IDriverRepository driverRepository,
            IProviderDriverRepository providerDriverRepository,
            IPersonRepository personRepository)
        {
            _driverRepository = driverRepository;
            _providerDriverRepository = providerDriverRepository;
            _personRepository = personRepository;
        }

        public async Task<CommandResponse<DriverExcelImportResult>> Handle(ImportDriverExcelCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
                return new NoAcess<DriverExcelImportResult> { Message = "Excel file is required." };

            var rows = DriverExcelService.Parse(request.File, request.StartRow, request.WorksheetName);
            var result = new DriverExcelImportResult
            {
                TotalRows = rows.Count
            };

            var providerDrivers = (await _providerDriverRepository.GetAsync(
                x => x.ProviderId == request.ProviderId,
                query => query.Include(x => x.Driver).ThenInclude(x => x.Person)))
                .ToList();

            var scopedDriversByEmail = providerDrivers
                .Where(x => !string.IsNullOrWhiteSpace(x.Driver?.Email))
                .GroupBy(x => NormalizeEmail(x.Driver.Email))
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
                    var email = NormalizeEmail(row.Email);
                    if (string.IsNullOrWhiteSpace(email))
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

                    if (!EmailPattern.IsMatch(email))
                    {
                        rowResult.Error = "The email format is invalid.";
                        result.Rows.Add(rowResult);
                        continue;
                    }

                    scopedDriversByEmail.TryGetValue(email, out var providerDriver);
                    var isCreate = providerDriver == null;

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

                        ApplyRow(driver, row);
                        driver = await _driverRepository.AddAsync(driver);
                        allDriversByEmail[email] = driver;
                    }
                    else
                    {
                        driver.Email = email;
                        driver.Password = string.IsNullOrWhiteSpace(row.Password) ? driver.Password : row.Password.Trim();
                        ApplyRow(driver, row);
                        await EnsurePersonAsync(driver, row);
                        await _driverRepository.UpdateAsync(driver);
                    }

                    if (providerDriver == null)
                    {
                        providerDriver = new ProviderDriver
                        {
                            ProviderId = request.ProviderId,
                            DriverId = driver.Id,
                            IsDefault = row.IsDefault ?? false
                        };
                        await _providerDriverRepository.AddAsync(providerDriver);
                        scopedDriversByEmail[email] = providerDriver;
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

                    await ResetOtherProviderDefaultsAsync(request.ProviderId, driver.Id, providerDriver.IsDefault);

                    rowResult.DriverId = driver.Id;
                    result.ImportedRows += 1;
                }
                catch (Exception ex)
                {
                    rowResult.Error = ex.Message;
                }

                result.Rows.Add(rowResult);
            }

            return new Success<DriverExcelImportResult> { Data = result };
        }

        private async Task EnsurePersonAsync(Driver driver, DriverExcelRowData row)
        {
            var person = driver.Person;
            if (person == null)
            {
                person = await _personRepository.GetByIdAsync(driver.PersonId);
                driver.Person = person;
            }

            if (person == null)
                return;

            person.FirstName = row.FirstName?.Trim() ?? person.FirstName;
            person.LastName = row.LastName?.Trim() ?? person.LastName;
            await _personRepository.UpdateAsync(person);
        }

        private static void ApplyRow(Driver driver, DriverExcelRowData row)
        {
            driver.License = string.IsNullOrWhiteSpace(row.License) ? driver.License : row.License.Trim();
            driver.LicenseExp = row.LicenseExp ?? driver.LicenseExp;
            driver.BackgroundCheck = string.IsNullOrWhiteSpace(row.BackgroundCheck) ? driver.BackgroundCheck : row.BackgroundCheck.Trim();
            driver.BackgroundCheckExp = row.BackgroundCheckExp ?? driver.BackgroundCheckExp;
            driver.ProfilePhoto = string.IsNullOrWhiteSpace(row.ProfilePhoto) ? driver.ProfilePhoto : row.ProfilePhoto.Trim();
            driver.Status = ParseStatus(row.Status) ?? driver.Status;
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

        private static string NormalizeEmail(string? email)
        {
            return (email ?? string.Empty).Trim().ToLowerInvariant();
        }

        private static DriverStatus? ParseStatus(string? rawStatus)
        {
            if (string.IsNullOrWhiteSpace(rawStatus))
                return null;

            if (Enum.TryParse<DriverStatus>(rawStatus.Trim(), true, out var enumStatus))
                return enumStatus;

            if (int.TryParse(rawStatus.Trim(), out var statusValue) && Enum.IsDefined(typeof(DriverStatus), statusValue))
                return (DriverStatus)statusValue;

            return null;
        }
    }
}
