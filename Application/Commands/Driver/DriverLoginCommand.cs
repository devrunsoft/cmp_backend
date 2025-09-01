using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class DriverLoginCommand : IRequest<CommandResponse<DriverResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

