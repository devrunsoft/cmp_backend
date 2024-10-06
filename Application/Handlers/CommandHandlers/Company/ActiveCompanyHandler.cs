using System;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Handlers
{
    public class ActiveCompanyHandler : IRequestHandler<ActivateCompanyCompany, CommandResponse<CompanyResponse>>
    {
        private readonly ICompanyRepository _companyRepository;

        public ActiveCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<CompanyResponse>> Handle(ActivateCompanyCompany request, CancellationToken cancellationToken)
        {
            Company company = (await _companyRepository.GetAsync(p=>p.ActivationLink== request.activationLink)).FirstOrDefault();

            if (company == null)
            {
                return new NoAcess<CompanyResponse>();
            }
            else if (company.Registered)
            {
                return new NoAcess<CompanyResponse>();
            }
            else 
            {
                company.Registered = true;
                await _companyRepository.UpdateAsync(company);
            }

            return new Success<CompanyResponse>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
        }
    }
}

