using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminConfigGetCommand : IRequest<CommandResponse<Config>>
    {
        public AdminConfigGetCommand()
        {
        }
    }
}

