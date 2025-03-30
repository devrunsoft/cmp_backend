using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers
{
    public class GetAllSiganbleCompanyContractHandler : IRequestHandler<GetAllSiganbleCompanyContractCommand, CommandResponse<List<CompanyContract>>>
    {
        private readonly ICompanyContractRepository _repository;
        public GetAllSiganbleCompanyContractHandler(ICompanyContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<CompanyContract>>> Handle(GetAllSiganbleCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _repository.GetAsync(p => p.CompanyId == request.CompanyId && p.Status == (int)CompanyContractStatus.Created)).ToList();
            return new Success<List<CompanyContract>>() { Data = invoices };
        }
    }
}

