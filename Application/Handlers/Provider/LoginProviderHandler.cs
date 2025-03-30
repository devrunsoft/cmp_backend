using System;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Repositories;
using System.Linq;
using ScoutDirect.Core.Caching;

namespace CMPNatural.Application.Handlers.Admin.Auth
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
            if (admin.Password != request.Password)
            {
                return new NoAcess<Provider>() { Message = "Login failed. Please check your username and password and try again." };
            }

            return new Success<Provider>() { Data = admin };


        }

    }
}

