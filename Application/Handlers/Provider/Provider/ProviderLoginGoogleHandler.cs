using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using Google.Apis.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Application.Handlers.Admin.Auth
{
    public class ProviderLoginGoogleHandler : IRequestHandler<ProviderLoginGoogleCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _reposiotry;
        private readonly IConfiguration _configuration;

        public ProviderLoginGoogleHandler(
            IProviderReposiotry adminRepository,
            IConfiguration configuration)
        {
            _reposiotry = adminRepository;
            _configuration = configuration;
        }

        public async Task<CommandResponse<Provider>> Handle(ProviderLoginGoogleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Credential))
            {
                return new NoAcess<Provider> { Message = "Google credential is required." };
            }

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.Credential,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[]
                        {
                            _configuration["GoogleAuth:ClientId"]
                        }
                    });
            }
            catch
            {
                return new NoAcess<Provider> { Message = "Invalid Google credential." };
            }

            var admin = (await _reposiotry.GetAsync(p => p.Email == payload.Email)).FirstOrDefault();

            if (admin == null)
            {
                return new NoAcess<Provider>() { Message = "Login failed. Please check your username and password and try again." };
            }
            if (admin.Status == Core.Enums.ProviderStatus.Blocked)
            {
                return new NoAcess<Provider>() { Message = "Your account is blocked. Please contact support for assistance." };
            }
            if (admin.Status == Core.Enums.ProviderStatus.PendingEmail)
            {
                return new NoAcess<Provider>()
                {
                    Message = "Your account is pending email verification. Please check your inbox and click the activation link to continue.",
                    Data = admin
                };
            }

            bool HasLogin = true;
            if (admin.HasLogin != true)
            {
                HasLogin = false;
            }

            admin.HasLogin = true;
            await _reposiotry.UpdateAsync(admin);
            admin.HasLogin = HasLogin;
            return new Success<Provider>() { Data = admin };
        }
    }
}
