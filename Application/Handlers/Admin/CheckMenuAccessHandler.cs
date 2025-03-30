using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin;
using ScoutDirect.Core.Repositories.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ScoutDirect.Core.Caching;

namespace CMPNatural.Application.Handlers.Admin
{
    public class CheckMenuAccessHandler : IRequestHandler<CheckMenuAccessQuery, CommandResponse<bool>>
    {
        private readonly IAdminMenuAccessRepository _repository;
        private readonly ICacheService _cache;

        public CheckMenuAccessHandler(IAdminMenuAccessRepository repository, Func<CacheTech, ICacheService> _cacheService)
        {
            _repository = repository;
            _cache = _cacheService(CacheTech.Memory);
        }

        public async Task<CommandResponse<bool>> Handle(CheckMenuAccessQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"menu_access_{request.AdminId}";

            // Check if the data is already cached
            if (!_cache.TryGet(cacheKey, out HashSet<long> userMenuIds))
            {
                // If not cached, fetch from the database
                userMenuIds = (await _repository.GetAsync(p => p.AdminId == request.AdminId)).Select(x => x.MenuId).ToHashSet();

                // Cache the result for 10 minutes
                _cache.Set(cacheKey, userMenuIds);
            }

            bool hasAccess = userMenuIds.Contains(request.MenuId);
            if (hasAccess)
            {
                return new Success<bool>() { Data = true };
            }
            else
            {
                return new NoAcess<bool>() { Data = false };
            }

        }

    }
}

