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
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace CMPNatural.Application.Handlers.QueryHandlers
{
    public class LoginGoogleCompanyHandler : IRequestHandler<LoginGoogleCompanyCommand, CommandResponse<CompanyResponse>>
    {
        private readonly ICompanyRepository _personRepository;
        private readonly IOperationalAddressRepository _operationalAddressRepository;
        private readonly IConfiguration _configuration;

        public LoginGoogleCompanyHandler(ICompanyRepository personRepository, IOperationalAddressRepository operationalAddressRepository, IConfiguration configuration)
        {
            _personRepository = personRepository;
            _operationalAddressRepository = operationalAddressRepository;
            _configuration = configuration;

        }

        public async Task<CommandResponse<CompanyResponse>> Handle(LoginGoogleCompanyCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Credential))
            {
                return new NoAcess<CompanyResponse> { Message = "Google credential is required." };
            }

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.Credential,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[]
                        {
                            _configuration["GoogleAuth:ClientId"]
                        }
                    });
            }
            catch
            {
                return new NoAcess<CompanyResponse> { Message = "Invalid Google credential." };
            }

            var person = (await _personRepository.GetAsync(x => x.BusinessEmail == payload.Email)).FirstOrDefault();

            if (person == null)
            {
                var operationalAddress = (await _operationalAddressRepository.GetAsync(x =>
                        x.Username == payload.Email))
                    .FirstOrDefault();

                if (operationalAddress == null)
                {
                    return new CommandResponse<CompanyResponse>() { Success = false, Message = "Account not found. Please check your username and password or register." };
                }

                var company = await _personRepository.GetByIdAsync(operationalAddress.CompanyId);
                if (company == null)
                {
                    return new CommandResponse<CompanyResponse>() { Success = false, Message = "Account not found. Please check your username and password or register." };
                }

                if (company.Status == CompanyStatus.Blocked)
                {
                    return new CommandResponse<CompanyResponse>() { Success = false, Message = "Your account is currently blocked. Please contact support for assistance." };
                }

                var companyResponse = CompanyMapper.Mapper.Map<CompanyResponse>(company);
                companyResponse.OperationalAddressId = operationalAddress.Id;

                return new CommandResponse<CompanyResponse>()
                {
                    Success = true,
                    Message = "Login successful! Redirecting...",
                    Data = companyResponse
                };
            }

            if (person.Status == CompanyStatus.Blocked)
            {
                return new CommandResponse<CompanyResponse>() { Success = false, Message = "Your account is currently blocked. Please contact support for assistance." };
            }
   

            return new CommandResponse<CompanyResponse>() { Success = true, Message = "Login successful! Redirecting...", Data = CompanyMapper.Mapper.Map<CompanyResponse>(person) };




        }
    }
}
