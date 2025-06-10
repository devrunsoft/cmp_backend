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
using CMPNatural.Application.Commands.Admin.Company;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Helper;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AdminAddCompanyHandler : IRequestHandler<AdminAddCompanyCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IPersonRepository _personRepository;

        public AdminAddCompanyHandler(ICompanyRepository companyRepository, IPersonRepository personRepository)
        {
            _companyRepository = companyRepository;
            _personRepository = personRepository;
        }

        public async Task<CommandResponse<object>> Handle(AdminAddCompanyCommand request, CancellationToken cancellationToken)
        {
            var emailPattern = @"^[\w\.\+-]+@([\w-]+\.)+[a-zA-Z]{2,7}$";
            if (!Regex.IsMatch(request.BusinessEmail, emailPattern))
            {
                return new NoAcess() { Message = "The email format is invalid. Please provide a valid email address." };
            }

            var companyExist = await _companyRepository.GetByEmailAsync(request.BusinessEmail);

            if (companyExist != null)
            {
                return new NoAcess() { Message = "A company with this email already exists." };
            }

            var personId = Guid.NewGuid();
            var person = new Person() { FirstName = request.PrimaryFirstName, LastName = request.PrimaryLastName, Id = personId };
            await _personRepository.AddAsync(person);

            var company = new Core.Entities.Company() { 
              CompanyName = request.CompanyName,
              Position = request.Position,
              PrimaryFirstName = request.PrimaryFirstName,
              PrimaryLastName = request.PrimaryLastName,
              PrimaryPhonNumber = request.PrimaryPhonNumber,
              SecondaryFirstName = request.SecondaryFirstName,
              SecondaryLastName = request.SecondaryLastName,
              SecondaryPhoneNumber = request.SecondaryPhoneNumber,
              BusinessEmail = request.BusinessEmail,
              Type = (int)CompanyType.Chain,
              PersonId = personId,
              Status = CompanyStatus.Approved,
              AccountNumber = "",
              ReferredBy = "",
              Registered=true,
              Accepted = true,
              Password = PasswordGenerator.GenerateSecurePassword()
             };

                await _companyRepository.UpdateAsync(company);
                return new Success<object>() { Data = company };

        }

    }
}
