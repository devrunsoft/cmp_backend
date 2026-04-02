using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class DriverGetProviderHandler : IRequestHandler<DriverGetProviderCommand, CommandResponse<List<Provider>>>
    {
        private readonly IProviderDriverRepository _repository;
        public DriverGetProviderHandler(IProviderDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<Provider>>> Handle(DriverGetProviderCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Id == request.DriverId,
            query => query.Include(x => x.Provider))).Select(x=>x.Provider).ToList();
            return new Success<List<Provider>>() { Data = result };
        }
    }
}

