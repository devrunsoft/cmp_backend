using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin
{
    public class AdminTwoFactorCommand : IRequest<CommandResponse<AdminEntity>>
    {
        public long AdminId { get; set; }
        public string Code { get; set; }
    }
}

