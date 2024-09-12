using Barbara.Core.Entities;
using Barbara.Application.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Core.Entities;
using ScoutDirect.Application.Responses;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class RegisterCompanyHandler : IRequestHandler<RegisterCompanyCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;

        public RegisterCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<object>> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByEmailAsync(request.BusinessEmail);

            if (company == null || !company.Registered)
            {
                company = new Company()
                {
                    AccountNumber=request.AccountNumber,
                    BusinessEmail= request.BusinessEmail,
                    CompanyName= request.CompanyName,
                    Position= request.Position,
                    PrimaryFirstName= request.PrimaryFirstName,
                    PrimaryLastName= request.PrimaryLastName,
                    PrimaryPhonNumber= request.PrimaryPhonNumber,
                    ReferredBy= request.ReferredBy,
                    SecondaryFirstName= request.SecondaryFirstName,
                    SecondaryLastName= request.SecondaryLastName,
                    SecondaryPhoneNumber= request.SecondaryPhoneNumber,
                    Registered=false,
                    Type=(int)request.Type,
                    Password=request.Password

                };

                await _companyRepository.AddAsync(company);
                return new Success<object>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };
            }

            return new NoAcess();
        }

    }
}
