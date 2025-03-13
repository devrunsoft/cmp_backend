using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application.Commands.Admin
{
    public class CheckMenuAccessQuery : IRequest<CommandResponse<bool>>
    {
        public CheckMenuAccessQuery(long AdminId, long MenuId)
        {
            this.AdminId = AdminId;
            this.MenuId = MenuId;
        }
        public long AdminId { get; set; }
        public long MenuId { get; set; }
    }
}

