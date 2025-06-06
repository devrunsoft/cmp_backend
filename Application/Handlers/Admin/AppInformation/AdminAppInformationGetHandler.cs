using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using System.Collections.Generic;
using ScoutDirect.Core.Caching;

namespace CMPNatural.Application
{
    public class AdminAppInformationGetHandler : IRequestHandler<AdminAppInformationGetCommand, CommandResponse<AppInformation>>
    {
        private readonly IAppInformationRepository _repository;
        private readonly ICacheService _cache;
        public AdminAppInformationGetHandler(IAppInformationRepository providerReposiotry, Func<CacheTech, ICacheService> _cacheService)
        {
            _repository = providerReposiotry;
            _cache = _cacheService(CacheTech.Memory);
        }
        public async Task<CommandResponse<AppInformation>> Handle(AdminAppInformationGetCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAllAsync()).LastOrDefault();

            var cacheKey = $"AppInformation";
            _cache.Set(cacheKey, entity);

            return new Success<AppInformation>() { Data = entity };
        }
    }
}

