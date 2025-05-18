using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Application.Services;
using System.IO;
using ScoutDirect.Core.Caching;
using CMPNatural.Application.Commands;
using Stripe.Tax;
using CMPNatural.Core.Models;

namespace CMPNatural.Application
{
    public class AdminGoHighLevelPutHandler : IRequestHandler<AdminGoHighLevelPutCommand, CommandResponse<GoHighLevel>>
    {
        private readonly IGoHighLevelRepository _repository;
        private readonly ICacheService _cache;
        private readonly HighLevelSettings _highLevelSetting;
        public AdminGoHighLevelPutHandler(IGoHighLevelRepository _repository, Func<CacheTech, ICacheService> _cacheService, HighLevelSettings _highLevelSetting)
        {
            this._highLevelSetting = _highLevelSetting;
            this._repository = _repository;
            _cache = _cacheService(CacheTech.Memory);
        }
        public async Task<CommandResponse<GoHighLevel>> Handle(AdminGoHighLevelPutCommand request, CancellationToken cancellationToken)
        {

            var entity = new GoHighLevel()
            {
                Authorization = request.Authorization,
                LocationId = request.locationId,
                RestApi = request.RestApi,
                Version = request.Version
            };

            entity = await _repository.AddAsync(entity);

            _highLevelSetting.update(entity);
            //var cacheKey = $"GoHighLevel";
            //_cache.Set(cacheKey, entity);

            return new Success<GoHighLevel>() { Data = entity };

        }
    }
}

