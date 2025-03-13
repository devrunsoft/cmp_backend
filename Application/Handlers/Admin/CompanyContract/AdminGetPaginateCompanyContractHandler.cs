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
            var invoices = (await _repository.GetBasePagedAsync(request, p => request.Status == null || p.Status == (int)request.Status,
             query =>query.Include(x=>x.Company)
            ));
            return new Success<PagesQueryResponse<CompanyContract>>() { Data = invoices };
        }
    }
}

