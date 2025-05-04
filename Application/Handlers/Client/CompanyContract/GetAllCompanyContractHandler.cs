using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Commands;
using System.Linq;
using System.Collections.Generic;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class GetAllCompanyContractHandler : IRequestHandler<GetAllCompanyContractCommand, CommandResponse<PagesQueryResponse<CompanyContract>>>
    {
        private readonly ICompanyContractRepository _repository;
        public GetAllCompanyContractHandler(ICompanyContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<CompanyContract>>> Handle(GetAllCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _repository.GetBasePagedAsync(request, p =>  p.CompanyId == request.CompanyId && p.Status != CompanyContractStatus.Created));
            return new Success<PagesQueryResponse<CompanyContract>>() { Data = invoices };
        }
    }
}

