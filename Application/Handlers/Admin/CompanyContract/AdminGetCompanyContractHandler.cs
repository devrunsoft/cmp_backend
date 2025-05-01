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
    public class AdminGetCompanyContractHandler : IRequestHandler<AdminGetCompanyContractCommand, CommandResponse<CompanyContract>>
    {
        private readonly ICompanyContractRepository _repository;

        public AdminGetCompanyContractHandler(ICompanyContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<CompanyContract>> Handle(AdminGetCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var model = (await _repository.GetByIdAsync(request.Id));
            return new Success<CompanyContract>() { Data = model };
        }
    }
}

