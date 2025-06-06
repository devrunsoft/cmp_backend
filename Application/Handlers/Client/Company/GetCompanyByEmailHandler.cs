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

namespace CMPNatural.Application.Handlers.CommandHandlers.Company
{
    public class GetCompanyByEmailHandler : IRequestHandler<GetCompanyByEmailCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;

        public GetCompanyByEmailHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<object>> Handle(GetCompanyByEmailCommand request, CancellationToken cancellationToken)
        {
            var company = (await _companyRepository.GetAsync(p=>p.BusinessEmail== request.Email )).FirstOrDefault();

            if (company != null)
            {
                return new Success<object>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
            }
            else
            {
                return new NoAcess() { Message = $"No company found with the email address" };
            }
        }
    }
}

