using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Provider.WhiteLabel;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ProviderGetAvailableTenantAccessHandler : IRequestHandler<ProviderGetAvailableTenantAccessCommand, CommandResponse<List<NameAndValue<long>>>>
    {
        private readonly ITenantRepository _tenantRepository;

        public ProviderGetAvailableTenantAccessHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<CommandResponse<List<NameAndValue<long>>>> Handle(ProviderGetAvailableTenantAccessCommand request, CancellationToken cancellationToken)
        {
            var result = (await _tenantRepository.GetAsync(x => x.IsActive && x.AllowMainAdminAccess))
                .Select(x => new NameAndValue<long>()
                {
                    name = x.Name,
                    value = x.Id
                })
                .ToList();

            return new Success<List<NameAndValue<long>>>() { Data = result };
        }
    }
}
