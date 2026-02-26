using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CMPNatural.Application
{
    public class ProviderGetPaginateProviderContractHandler : IRequestHandler<ProviderGetPaginateProviderContractCommand, CommandResponse<PagesQueryResponse<ProviderContract>>>
    {
        private readonly IProviderContractRepository _repository;

        public ProviderGetPaginateProviderContractHandler(IProviderContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<ProviderContract>>> Handle(ProviderGetPaginateProviderContractCommand request, CancellationToken cancellationToken)
        {
            var allField = request.allField?.Trim();
            var invoices = (await _repository.GetBasePagedAsync(request,
             p =>
                (request.ProviderId == null || p.ProviderId == request.ProviderId) &&
                (request.Status == null || p.Status == request.Status) &&
                (string.IsNullOrWhiteSpace(allField) ||
                 ("PC" + p.CreatedAt.Year + "-" + p.CompanyId + "/" + p.OperationalAddressId + "-" + p.Id).Contains(allField))
             ,
             query => query.Include(x => x.Company)
            ));
            return new Success<PagesQueryResponse<ProviderContract>>() { Data = invoices };
        }
    }
}
