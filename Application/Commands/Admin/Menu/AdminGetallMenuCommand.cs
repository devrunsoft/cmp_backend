using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{

    public class AdminGetallMenuCommand : IRequest<CommandResponse<List<Menu>>>
    {
        public AdminGetallMenuCommand()
        {
        }
    }
}

