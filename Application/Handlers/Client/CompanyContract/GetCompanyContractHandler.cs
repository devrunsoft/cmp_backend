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

namespace CMPNatural.Application
{
    public class GetCompanyContractHandler : IRequestHandler<GetCompanyContractCommand, CommandResponse<CompanyContract>>
    {
        private readonly ICompanyContractRepository _repository;
        public GetCompanyContractHandler(ICompanyContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<CompanyContract>> Handle(GetCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var invoice = (await _repository.GetAsync(p => p.CompanyId == request.CompanyId && p.Id == request.ContractId)).FirstOrDefault();
            return new Success<CompanyContract>() { Data = invoice };
        }
    }
}

