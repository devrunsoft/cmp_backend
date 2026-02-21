using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Company;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class RequestCompanyEmailChangeHandler : IRequestHandler<RequestCompanyEmailChangeCommand, CommandResponse<string>>
    {
        private readonly ICompanyRepository _companyRepository;

        public RequestCompanyEmailChangeHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<string>> Handle(RequestCompanyEmailChangeCommand request, CancellationToken cancellationToken)
        {
            var email = (request.Email ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(email))
                return new NoAcess<string>() { Message = "Email is required." };

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company == null)
                return new NoAcess<string>() { Message = "Company not found." };

            if (!string.IsNullOrWhiteSpace(company.BusinessEmail) &&
                string.Equals(company.BusinessEmail.Trim(), email, StringComparison.OrdinalIgnoreCase))
            {
                return new NoAcess<string>() { Message = "This email is already associated with your account." };
            }

            var code = GenerateCode();
            company.PendingEmail = email;
            company.EmailChangeCode = code;

            await _companyRepository.UpdateAsync(company);

            return new Success<string>() { Data = code };
        }

        private static string GenerateCode()
        {
            var value = RandomNumberGenerator.GetInt32(100000, 1000000);
            return value.ToString();
        }
    }
}
