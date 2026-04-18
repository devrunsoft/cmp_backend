using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class CreateDriverForgotPasswordLinkCommand : IRequest<CommandResponse<Driver>>
    {
        public string Email { get; set; }
    }
}
