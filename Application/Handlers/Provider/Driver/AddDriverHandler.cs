using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Mapper;
using CMPNatural.Core.Helper;
using CMPFile;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CMPNatural.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class AddDriverHandler : IRequestHandler<AddDriverCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;
        private readonly IProviderDriverRepository _providerDriverRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IFileStorage fileStorage;
        private readonly AppSetting _appSetting;
        private readonly SsoOptions _ssoOptions;

        public AddDriverHandler(
            IDriverRepository repository,
            IProviderDriverRepository providerDriverRepository,
            IPersonRepository personRepository,
            IFileStorage fileStorage,
            AppSetting appSetting,
            IOptions<SsoOptions> ssoOptions)
        {
            _repository = repository;
            _providerDriverRepository = providerDriverRepository;
            _personRepository = personRepository;
            this.fileStorage = fileStorage;
            _appSetting = appSetting;
            _ssoOptions = ssoOptions?.Value ?? new SsoOptions();
        }

        public async Task<CommandResponse<DriverResponse>> Handle(AddDriverCommand request, CancellationToken cancellationToken)
        {
            Driver driverEntity;
            var existingDriver = (await _repository.GetAsync(
                x => x.Email == request.Email,
                query => query.Include(x => x.ProviderDriver).Include(x => x.Person))).FirstOrDefault();

            if (existingDriver != null)
            {
                driverEntity = existingDriver;

                if (!string.IsNullOrWhiteSpace(request.ProfilePhoto))
                {
                    existingDriver.License = request.ProfilePhoto;
                }
                existingDriver.LicenseExp = request.LicenseExp;

                if (!string.IsNullOrWhiteSpace(request.BackgroundCheck))
                {
                    existingDriver.BackgroundCheck = request.BackgroundCheck;
                }
                existingDriver.BackgroundCheckExp = request.BackgroundCheckExp;

                if (!string.IsNullOrWhiteSpace(request.ProfilePhoto))
                    existingDriver.ProfilePhoto = request.ProfilePhoto;

                existingDriver.Person.FirstName = request.FirstName;
                existingDriver.Person.LastName = request.LastName;
                existingDriver.Email = request.Email;

                var relation = existingDriver.ProviderDriver
                    .FirstOrDefault(x => x.ProviderId == request.ProviderId && x.DriverId == existingDriver.Id);

                if (relation == null)
                {
                    relation = new ProviderDriver
                    {
                        ProviderId = request.ProviderId,
                        DriverId = existingDriver.Id,
                        IsDefault = request.IsDefault
                    };
                    existingDriver.ProviderDriver.Add(relation);
                }
                else
                {
                    relation.IsDefault = request.IsDefault;
                }

                await ResetOtherProviderDefaultsAsync(request.ProviderId, existingDriver.Id, request.IsDefault);
                await _repository.UpdateAsync(existingDriver);
            }
            else
            {
                var personId = Guid.NewGuid();
                var person = new Person() { FirstName = request.FirstName, LastName = request.LastName, Id = personId };

                var entity = new Driver()
                {
                    License = request.License,
                    LicenseExp = request.LicenseExp,
                    BackgroundCheck = request.BackgroundCheck,
                    BackgroundCheckExp = request.BackgroundCheckExp,
                    ProfilePhoto = request.ProfilePhoto,
                    ProviderDriver = new List<ProviderDriver>() { new ProviderDriver() { ProviderId = request.ProviderId, IsDefault = request.IsDefault } },
                    Password = PasswordGenerator.GenerateSecurePassword(),
                    Email = request.Email,
                    Person = person
                };

                var result = await _repository.AddAsync(entity);
                await ResetOtherProviderDefaultsAsync(request.ProviderId, result.Id, request.IsDefault);
                driverEntity = result;
            }

            var providerDriver = await LoadProviderDriverAsync(request.ProviderId, driverEntity.Id);

            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(providerDriver) };
        }

        private async Task ResetOtherProviderDefaultsAsync(long providerId, long driverId, bool shouldReset)
        {
            if (!shouldReset)
            {
                return;
            }

            var otherRelations = await _providerDriverRepository.GetAsync(x => x.ProviderId == providerId && x.DriverId != driverId && x.IsDefault);
            foreach (var relation in otherRelations)
            {
                relation.IsDefault = false;
                await _providerDriverRepository.UpdateAsync(relation);
            }
        }

        private async Task<ProviderDriver> LoadProviderDriverAsync(long providerId, long driverId)
        {
            return (await _providerDriverRepository.GetAsync(
                x => x.ProviderId == providerId && x.DriverId == driverId,
                query => query.Include(x => x.Driver).ThenInclude(x => x.Person))).FirstOrDefault();
        }

        private async Task<CommandResponse<DriverResponse>> UpdateSsoUserAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new NoAcess<DriverResponse>() { Message = "Please try again later" };
            }

            var serverUrl = _appSetting?.host;
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                return new NoAcess<DriverResponse>() { Message = "Please try again later" };
            }

            var serverName = serverUrl;
            if (Uri.TryCreate(serverUrl, UriKind.Absolute, out var serverUri))
            {
                serverName = serverUri.Host;
            }
            else if (Uri.TryCreate("http://" + serverUrl, UriKind.Absolute, out var fallbackUri))
            {
                serverName = fallbackUri.Host;
            }

            var payload = new
            {
                Email = email,
                ServerName = serverName,
                ServerUrl = serverUrl
            };

            var body = JsonSerializer.Serialize(payload);
            var requestUri = new Uri("https://sso.app-cmp.com/sso/api/sso/users/update");
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var signature = CreateSignature("POST", "/api/sso/users/update", body, timestamp, _ssoOptions.SharedSecret);

            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            request.Headers.TryAddWithoutValidation("X-Sso-Timestamp", timestamp.ToString());
            request.Headers.TryAddWithoutValidation("X-Sso-Signature", signature);

            var response = await client.SendAsync(request, cancellationToken);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new NoAcess<DriverResponse>() { Message = "Please try again later" };
            }
            else
            {
                return new Success<DriverResponse>();
            }
        }

        private static string CreateSignature(string method, string pathAndQuery, string body, long timestamp, string secret)
        {
            var canonical = $"{timestamp}.{method.ToUpperInvariant()}.{pathAndQuery}.{body}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret ?? string.Empty));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(canonical));
            return Convert.ToHexString(hash);
        }
    }
}
