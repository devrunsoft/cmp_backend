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
    public class AdminGetPaginateCompanyContractHandler : IRequestHandler<AdminGetPaginateCompanyContractCommand, CommandResponse<PagesQueryResponse<CompanyContract>>>
    {
        private readonly ICompanyContractRepository _repository;

        public AdminGetPaginateCompanyContractHandler(ICompanyContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<CompanyContract>>> Handle(AdminGetPaginateCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var allField = request.allField?.Trim();
            var invoices = (await _repository.GetBasePagedAsync(request,
             p =>
                (request.CompanyId == null || p.CompanyId == request.CompanyId) &&
                (request.Status == null || p.Status == request.Status) &&
                (string.IsNullOrWhiteSpace(allField) ||
                 ("C" + p.CreatedAt.Year + "-" + p.CompanyId + "/" + p.OperationalAddressId + "-" + p.Id).Contains(allField))
             ,
             query =>query.Include(x=>x.Company)
            ));
            return new Success<PagesQueryResponse<CompanyContract>>() { Data = invoices };
        }
    }
}
