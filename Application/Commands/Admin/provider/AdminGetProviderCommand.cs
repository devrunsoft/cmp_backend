using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Commands.Admin.provider

{ 
    public class AdminGetProviderCommand :IRequest<CommandResponse<Provider>>
    {
        public long Id { get; set; }
    }
}

