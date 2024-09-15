using System;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;

namespace CMPNatural.Application.Handlers
{

    public class CheckLinkCompanyHandler : IRequestHandler<CheckLinkCompanyCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;

        public CheckLinkCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<object>> Handle(CheckLinkCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company == null)
            {
                return new NoAcess() { };
            }

            company.ActivationLink = request.forgotPasswordLink;
            await _companyRepository.UpdateAsync(company);

            return new Success<object>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
        }
    }
}

