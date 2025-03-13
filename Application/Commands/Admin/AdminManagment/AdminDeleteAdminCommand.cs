using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.AdminManagment
{
    public class AdminDeleteAdminCommand : AdminInput, IRequest<CommandResponse<AdminEntity>>
    {
        public AdminDeleteAdminCommand()
        {
        }
        public long Id { get; set; }
    }
}

