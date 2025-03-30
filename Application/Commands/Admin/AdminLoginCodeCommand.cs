using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin
{
    public class AdminLoginCodeCommand : IRequest<CommandResponse<AdminEntity>>
    {
        public string Email { get; set; }
        public string Code { get; set; }

    }
}

