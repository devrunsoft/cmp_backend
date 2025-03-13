using System;
using System.Collections.Generic;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.Menu
{

    public class AdminAddMenuCommand : IRequest<CommandResponse<AdminEntity>>
    {
        public AdminAddMenuCommand()
        {
        }

        public List<int> MenuIds { get; set; }
        public long AdminId { get; set; }
    }
}

