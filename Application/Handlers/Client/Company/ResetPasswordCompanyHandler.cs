using System;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application.Handlers.CommandHandlers.Company
{
    public class ResetPasswordCompanyHandler : IRequestHandler<ResetPasswordCompanyCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;

        public ResetPasswordCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<object>> Handle(ResetPasswordCompanyCommand request, CancellationToken cancellationToken)
        {

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);

            if (company.ActivationLink == null)
            {
                return new NoAcess() { };
            }

            company.Password = request.Password;

            company.ActivationLink = null;

            await _companyRepository.UpdateAsync(company);


            return new Success<object>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
        }
    }
}

