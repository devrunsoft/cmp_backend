using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Application.Responses.AdminMenuRepresentation;

namespace CMPNatural.Application
{
    public class AdminMenuRepresentationCommand : IRequest<CommandResponse<AdminMenuRepresentationResponse>>
    {
        public AdminMenuRepresentationCommand()
        {
        }
    }
}

