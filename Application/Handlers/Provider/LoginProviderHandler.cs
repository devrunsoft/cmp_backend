using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using ScoutDirect.Core.Caching;

namespace CMPNatural.Application
{

    public class LoginProviderHandler : IRequestHandler<LoginProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _reposiotry;
        private readonly ICacheService _cache;

        public LoginProviderHandler(IProviderReposiotry _reposiotry, Func<CacheTech, ICacheService> _cacheService)
        {
            this._reposiotry = _reposiotry;
            _cache = _cacheService(CacheTech.Memory);
        }

        public async Task<CommandResponse<Provider>> Handle(LoginProviderCommand request, CancellationToken cancellationToken)
        {
            var admin = (await _reposiotry.GetAsync(p => p.Email == request.Email)).FirstOrDefault();

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
                    Message = "Your account is pending email verification. Please check your inbox and click the activation link to continue."
                };
            }
            if (admin.Password != request.Password)
            {
                return new NoAcess<Provider>() { Message = "Login failed. Please check your username and password and try again." };
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

