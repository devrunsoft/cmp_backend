using Barbara.Application.Queries;
using Barbara.Application.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Application.Queries;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Application.Responses;
using ScoutDirect.Application.Responses.Base;
using CMPNatural.Application.Commands;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;

namespace CMPNatural.Application.Handlers.QueryHandlers
{
    public class GetLoginQuery : IRequestHandler<LoginCompanyCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _personRepository;
        private readonly IOperationalAddressRepository _operationalAddressRepository;

        public GetLoginQuery(ICompanyRepository personRepository, IOperationalAddressRepository operationalAddressRepository)
        {
            _personRepository = personRepository;
            _operationalAddressRepository = operationalAddressRepository;

        }

        public async Task<CommandResponse<object>> Handle(LoginCompanyCommand request, CancellationToken cancellationToken)
        {
            var person = (await _personRepository.GetAsync(x=>x.BusinessEmail == request.BusinessEmail && x.Password == request.Password)).FirstOrDefault();

            if (person == null)
            {
                var operationalAddress = (await _operationalAddressRepository.GetAsync(x =>
                        x.Username == request.BusinessEmail && x.Password == request.Password))
                    .FirstOrDefault();

                if (operationalAddress == null)
                {
                    return new CommandResponse<object>() { Success = false, Message= "Account not found. Please check your username and password or register." };
                }

                var company = await _personRepository.GetByIdAsync(operationalAddress.CompanyId);
                if (company == null)
                {
                    return new CommandResponse<object>() { Success = false, Message = "Account not found. Please check your username and password or register." };
                }

                if (company.Status == CompanyStatus.Blocked)
                {
                    return new CommandResponse<object>() { Success = false, Message = "Your account is currently blocked. Please contact support for assistance." };
                }

                var companyResponse = CompanyMapper.Mapper.Map<CompanyResponse>(company);
                companyResponse.OperationalAddressId = operationalAddress.Id;

                return new CommandResponse<object>()
                {
                    Success = true,
                    Message = "Login successful! Redirecting...",
                    Data = companyResponse
                };
            }

            if (person.Status == CompanyStatus.Blocked)
            {
                return new CommandResponse<object>() { Success = false, Message = "Your account is currently blocked. Please contact support for assistance." };
            }

            if (person.Password == request.Password) {
                return new CommandResponse<object>() { Success = true, Message = "Login successful! Redirecting...", Data = CompanyMapper.Mapper.Map<CompanyResponse>(person) };
            }

            else
            {
                return new CommandResponse<object>() { Success = false, Message = "Invalid email or password. Please try again." };
            }



        }
    }
}
