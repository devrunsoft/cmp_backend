using Barbara.Application.Responses;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class RegisterCompanyCommand : IRequest<CommandResponse<object>>
    {
        public string CompanyName { get; set; }
        public string PrimaryFirstName { get; set; }
        public string PrimaryLastName { get; set; }
        public string PrimaryPhonNumber { get; set; }
        public string BusinessEmail { get; set; }
        public string Position { get; set; }
        public string SecondaryFirstName { get; set; } = null;
        public string SecondaryLastName { get; set; } = null;
        public string SecondaryPhoneNumber { get; set; } = null;
        public string ReferredBy { get; set; }
        public string AccountNumber { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public CompanyType Type { get; set; }

    }
}
