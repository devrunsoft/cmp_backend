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
    public class AdminImportProviderExcelHandler : IRequestHandler<AdminImportProviderExcelCommand, CommandResponse<ProviderExcelImportResult>>
    {
        private static readonly Regex EmailPattern = new Regex(@"^[\w\.\+-]+@([\w-]+\.)+[a-zA-Z]{2,7}$", RegexOptions.Compiled);

        private readonly IProviderReposiotry _providerReposiotry;
        private readonly IProviderServiceRepository _providerServiceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPersonRepository _personRepository;

        public AdminImportProviderExcelHandler(
            IProviderReposiotry providerReposiotry,
            IProviderServiceRepository providerServiceRepository,
            IProductRepository productRepository,
            IPersonRepository personRepository)
        {
            _providerReposiotry = providerReposiotry;
            _providerServiceRepository = providerServiceRepository;
            _productRepository = productRepository;
            _personRepository = personRepository;
        }

        public async Task<CommandResponse<ProviderExcelImportResult>> Handle(AdminImportProviderExcelCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
                return new NoAcess<ProviderExcelImportResult>() { Message = "Excel file is required." };

            var rows = ProviderExcelService.Parse(request.File, request.StartRow, request.WorksheetName);
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
    }
}
