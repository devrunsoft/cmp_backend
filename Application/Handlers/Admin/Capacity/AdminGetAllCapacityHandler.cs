using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Enums;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminGetAllCapacityHandler : IRequestHandler<AdminGetAllCapacityCommand, CommandResponse<PagesQueryResponse<Capacity>>>
    {
        private readonly ICapacityRepository _repository;

        public AdminGetAllCapacityHandler(ICapacityRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }

        public async Task<CommandResponse<PagesQueryResponse<Capacity>>> Handle(AdminGetAllCapacityCommand request, CancellationToken cancellationToken)
        {
            var search = request.allField ?? string.Empty;

            var matches = Enum.GetValues(typeof(ServiceType))
                .Cast<ServiceType>()
                .Where(e =>
                    e.GetDescription()
                     .Replace("_", "", StringComparison.OrdinalIgnoreCase)
                     .Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
            if(matches.Count > 0)
            {
                request.allField = "";
            }

            var invoices = await _repository.GetBasePagedAsync(
                request,
                p =>
                    (request.Enable == null || p.Enable == request.Enable) &&
                    (matches.Count == 0 || matches.Contains((ServiceType)p.ServiceType)),
                null
            );
            return new Success<PagesQueryResponse<Capacity>>() { Data = invoices };
        }
    }
}

