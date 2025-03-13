using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin
{
    public class AdminDeleteContractCommand : IRequest<CommandResponse<Contract>>
    {
        public AdminDeleteContractCommand()
        {
        }
        public long Id { get; set; }
    }
}

