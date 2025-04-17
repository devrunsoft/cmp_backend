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
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers
{
    public class RegisterCompanyHandler : IRequestHandler<RegisterCompanyCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IPersonRepository _personRepository;

        public RegisterCompanyHandler(ICompanyRepository companyRepository, IPersonRepository personRepository)
        {
            _companyRepository = companyRepository;
            _personRepository = personRepository;
        }

        public async Task<CommandResponse<object>> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByEmailAsync(request.BusinessEmail);

            if (company != null)
            {
                return new NoAcess() { Message = "A company with this email already exists. Please use a different email or log in." };
            }


                var personId = Guid.NewGuid();
                var person = new Person() { FirstName = request.PrimaryFirstName, LastName = request.PrimaryLastName, Id = personId };
                await _personRepository.AddAsync(person);

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
                    Password = request.Password,
                    ActivationLink = Guid.NewGuid(),
                    PersonId = personId,
                    Status = CompanyStatus.Approved

                };

                await _companyRepository.AddAsync(company);
                return new Success<object>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(company) };

        }

    }
}
