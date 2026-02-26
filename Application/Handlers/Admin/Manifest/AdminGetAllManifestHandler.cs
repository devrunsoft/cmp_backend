using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminGetAllManifestHandler : IRequestHandler<AdminGetAllManifestCommand, CommandResponse<PagesQueryResponse<Manifest>>>
    {
        private readonly IManifestRepository _repository;
        private readonly IProviderContractRepository _providerContractRepository;

        public AdminGetAllManifestHandler(IManifestRepository repository, IProviderContractRepository providerContractRepository)
        {
            _repository = repository;
            _providerContractRepository = providerContractRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Manifest>>> Handle(AdminGetAllManifestCommand request, CancellationToken cancellationToken)
        {
            var filter = QueryManifestExtensions.FilterByQuery(
                request.allField,
                request.Status,
                request.startDate,
                request.endDate);

            var result = (await _repository.GetBasePagedAsync(request, filter,
            query => query
            .Include(x => x.Provider)
            .Include(x => x.Company)
            .Include(x => x.Request)
            .ThenInclude(x => x.Company)
             .Include(x => x.Request)
            .ThenInclude(x => x.Provider)
            , filterAll: false));

            var requestIds = result.elements.Select(x => x.RequestId).Distinct().ToList();
            var providerIds = result.elements
                .Where(x => x.ProviderId.HasValue)
                .Select(x => x.ProviderId!.Value)
                .Distinct()
                .ToList();

            if (requestIds.Any() && providerIds.Any())
            {
                var providerContracts = (await _providerContractRepository.GetAsync(
                    x => requestIds.Contains(x.RequestId) && providerIds.Contains(x.ProviderId)))
                    .OrderByDescending(x => x.Id)
                    .ToList();

                var providerContractMap = providerContracts
                    .GroupBy(x => new { x.RequestId, x.ProviderId })
                    .ToDictionary(g => (g.Key.RequestId, g.Key.ProviderId), g => g.First());

                foreach (var manifest in result.elements.Where(x => x.ProviderId.HasValue))
                {
                    if (providerContractMap.TryGetValue((manifest.RequestId, manifest.ProviderId!.Value), out var providerContract))
                    {
                        manifest.ProviderContract = providerContract;
                    }
                }
            }

            return new Success<PagesQueryResponse<Manifest>>() { Data = result };
        }
    }
}
