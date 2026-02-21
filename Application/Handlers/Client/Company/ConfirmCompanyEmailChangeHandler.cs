using System;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Company;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class ConfirmCompanyEmailChangeHandler : IRequestHandler<ConfirmCompanyEmailChangeCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository _companyRepository;

        public ConfirmCompanyEmailChangeHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CommandResponse<object>> Handle(ConfirmCompanyEmailChangeCommand request, CancellationToken cancellationToken)
        {
            var email = (request.Email ?? string.Empty).Trim();
            var code = (request.Code ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
                return new NoAcess<object>() { Message = "Email and code are required." };

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company == null)
                return new NoAcess<object>() { Message = "Company not found." };

            if (string.IsNullOrWhiteSpace(company.EmailChangeCode) ||
                !string.Equals(company.EmailChangeCode, code, StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(company.PendingEmail) ||
                !string.Equals(company.PendingEmail.Trim(), email, StringComparison.OrdinalIgnoreCase))
            {
                return new NoAcess<object>() { Message = "Invalid code or email." };
            }

            company.BusinessEmail = company.PendingEmail;
            company.PendingEmail = null;
            company.EmailChangeCode = null;

            await _companyRepository.UpdateAsync(company);

            return new Success<object>() { Data = true };
        }
    }
}
