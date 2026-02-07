using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Services;
using ScoutDirect.Core.Entities.Base;
using CMPNatural.Application.Mapper;
using CMPNatural.Core.Helper;
using CMPFile;
using System.ServiceModel.Channels;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CMPNatural.Core.Models;
using Microsoft.Extensions.Options;

namespace CMPNatural.Application
{
    public class AddDriverHandler : IRequestHandler<AddDriverCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;
        private readonly IPersonRepository _personRepository;
        private readonly IFileStorage fileStorage;
        private readonly AppSetting _appSetting;
        private readonly SsoOptions _ssoOptions;

        public AddDriverHandler(IDriverRepository repository, IPersonRepository _personRepository , IFileStorage fileStorage, AppSetting appSetting, IOptions<SsoOptions> ssoOptions)
        {
            _repository = repository;
            this._personRepository = _personRepository;
            this.fileStorage = fileStorage;
            _appSetting = appSetting;
            _ssoOptions = ssoOptions?.Value ?? new SsoOptions();
        }

        public async Task<CommandResponse<DriverResponse>> Handle(AddDriverCommand request, CancellationToken cancellationToken)
        {
            var existingDriver = (await _repository.GetAsync(x => x.Email == request.Email)).Any();
            if (existingDriver)
            {
                return new NoAcess<DriverResponse>() { Message = "A driver with this email already exists." };
            }

            var personId = Guid.NewGuid();
            var person = new Person() { FirstName = request.FirstName, LastName = request.LastName, Id = personId };
            //await _personRepository.AddAsync(person);

            var entity = new Driver() {
                License= request.License,
                LicenseExp = request.LicenseExp,
                BackgroundCheck = request.BackgroundCheck,
                BackgroundCheckExp = request.BackgroundCheckExp,
                ProfilePhoto= request.ProfilePhoto,
                ProviderId = request.ProviderId,
                Password = PasswordGenerator.GenerateSecurePassword(),
                Email = request.Email,
                IsDefault = request.IsDefault,
                Person = person
            };

            var result = await _repository.AddAsync(entity);

           var res= await UpdateSsoUserAsync(request.Email, cancellationToken);
            if (!res.IsSucces())
            {
             return new NoAcess<DriverResponse>(){ Message = "Please try again later"};
            }

            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(result) };
        }

        private async Task<CommandResponse<DriverResponse>> UpdateSsoUserAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                    return new NoAcess<DriverResponse>(){ Message = "Please try again later"};
            }

            var serverUrl = _appSetting?.host;
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                  return new NoAcess<DriverResponse>(){ Message = "Please try again later"};
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
            var requestUri = new Uri("http://monitor.app-cmp.com:8081/sso/api/sso/users/update");
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
                return new NoAcess<DriverResponse>(){ Message = "Please try again later"};
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
