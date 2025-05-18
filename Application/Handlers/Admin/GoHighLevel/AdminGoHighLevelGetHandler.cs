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
    public class AdminGoHighLevelGetHandler : IRequestHandler<AdminGoHighLevelGetCommand, CommandResponse<GoHighLevel>>
    {
        private readonly IGoHighLevelRepository _repository;
        private readonly ICacheService _cache;
        public AdminGoHighLevelGetHandler(IGoHighLevelRepository _repository, Func<CacheTech, ICacheService> _cacheService)
        {
            this._repository = _repository;
            _cache = _cacheService(CacheTech.Memory);
        }
        public async Task<CommandResponse<GoHighLevel>> Handle(AdminGoHighLevelGetCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAllAsync()).LastOrDefault();

            //var cacheKey = $"GoHighLevel";
            //_cache.Set(cacheKey, entity);

            return new Success<GoHighLevel>() { Data = entity };
        }
    }
}

