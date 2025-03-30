using System;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CMPNatural.Application.Handlers.CommandHandlers.Company
{
    public class ResendEmailCompanyHandler : IRequestHandler<ResendEmailCompanyCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;

        public ResendEmailCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<object>> Handle(ResendEmailCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company.Registered)
            {
                return new NoAcess() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
            }
                company.ActivationLink = request.ActivationLink;
                await _companyRepository.UpdateAsync(company);

            return new Success<object>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
        }
    }
}


